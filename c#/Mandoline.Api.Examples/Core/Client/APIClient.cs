using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Client;
using Core.Client.Models;
using Core.Client.Models.Requests;
using Core.Client.ServiceModels;
using Core.Client.Utilities;
using User = Core.Client.Models.User;

namespace Core;

public class ApiClient
{
    private readonly string _clientName = null;
    private MandolineWebRequestBuilder _requestBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient"/> class.
    /// Create new instance of the ApiClient using a specific url.
    /// </summary>
    public ApiClient(string url)
    {
        this.SetUrl(url);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient"/> class.
    /// Create new instance of the ApiClient using a specific url.
    /// </summary>
    public ApiClient(string url, string apiKey)
        : this(url)
    {
        this.SetApiKey(apiKey);
    }

    public void SetUrl(string url)
    {
        this._requestBuilder = new MandolineWebRequestBuilder(url, null, this._clientName, null);
    }

    /// <summary>
    /// Login user using an api key.
    /// </summary>
    /// <param name="apiKey">api key.</param>
    /// <returns>ApiClient.</returns>
    public ApiClient SetApiKey(string apiKey)
    {
        Trace.WriteLine("SetApiKey(" + apiKey + ")");
        this._requestBuilder.SetApiKey(apiKey);
        return this;
    }

    /// <summary>
    /// Login using username and password.
    /// </summary>
    /// <param name="username">username.</param>
    /// <param name="password">password.</param>
    /// <returns>User corresponding to provided username / password.</returns>
    public async Task<Assertion<User>> LoginAsync(string username, string password, CancellationToken cancel)
    {
        Trace.WriteLine("LoginAsync(" + username + ")");
        LoginParametersDto parameters = new LoginParametersDto
        {
            Username = username,
            Password = password,
        };

        // var loginUri = new UriBuilder(ResourceLinks.UserPath);
        // loginUri.Scheme = Uri.UriSchemeHttps;
        // loginUri.Port = 443;
        Assertion<User> user = await this.PerformPost<User>(parameters, ResourceLinks.UserPath, cancel);

        // update api key on successful login
        if (user.Passed)
        {
            user.Result.Client = this;
            Trace.WriteLine("ApiKey: " + user.Result.ApiKey);

            this.SetApiKey(user.Result.ApiKey);
        }

        return user;
    }

    public async Task<Assertion<Core.Client.Models.User>> GetUserAsync(string id = "me", CancellationToken cancel = default(CancellationToken))
    {
        Trace.WriteLine("Beginning GetUser(" + id + ")");
        Assertion<User> userAssertion = await this.PerformGet<Core.Client.Models.User>(ResourceLinks.UserPath + "/" + id, cancel);

        if (userAssertion.Passed)
        {
            userAssertion.Result.Client = this;
            Trace.WriteLine("GetUserSuccess: " + userAssertion.Result);
        }
        else
        {
            Trace.WriteLine("GetUserFailed:" + userAssertion.Reason + " " + userAssertion.Description);
        }

        return userAssertion;
    }

    public async Task<Assertion<List<Core.Client.Models.User>>> GetAllUsersAsync(CancellationToken cancel = default(CancellationToken))
    {
        Assertion<List<User>> userAssertion = await this.PerformGet<List<Core.Client.Models.User>>(ResourceLinks.UserPath, cancel);

        if (userAssertion.Passed)
        {
            foreach (User user in userAssertion.Result)
            {
                user.Client = this;
            }
        }

        return userAssertion;
    }

    /// <summary>
    /// Retrieve all databanks.
    /// </summary>
    /// <returns>list of databanks.</returns>
    public async Task<Assertion<IEnumerable<Core.Client.Models.Databank>>> GetDatabanksAsync()
    {
        Trace.WriteLine("GetDatabanksAsync()");
        string url = ResourceLinks.DatabankPath;

        Assertion<List<DatabankDto>> responseTask = await this.PerformGet<List<DatabankDto>>(url);

        if (!responseTask.Passed)
        {
            return Assertion<IEnumerable<Databank>>.Fail(null, responseTask.Reason, responseTask.Description);
        }

        IEnumerable<Databank> result =
            from databank in responseTask.Result
            select new Databank(this)
            {
                HasAccess = databank.HasAccess,
                DatabankCode = databank.DatabankCode,
                DatabankColumns = databank.DatabankColumns,
                HasQuarterlyData = databank.HasQuarterlyData,
                EndYear = databank.EndYear,
                MapUrl = databank.MapUrl,
                Name = databank.Name,
                StartYear = databank.StartYear,
                Url = databank.Url,
                Trees = from tree in databank.Trees
                        select new TreeLink(this)
                        {
                            Name = tree.Name,
                            TreeCode = tree.TreeCode,
                            Url = tree.Url,
                        },
            };

        return Assertion<IEnumerable<Databank>>.Pass(result, responseTask.Reason);
    }

    /// <summary>
    /// Create a new saved selection.
    /// </summary>
    /// <param name="selectionToCreate">selection to create.</param>
    /// <returns>created selection.</returns>
    public async Task<Assertion<SelectionDto>> CreateSavedSelectionAsync(SelectionDto selectionToCreate, CancellationToken cancel)
    {
        Trace.WriteLine("CreateSavedSelectionAsync(" + selectionToCreate.Id + ")");
        Assertion<SelectionDto> createdSelection = await this.PerformPost<SelectionDto>(selectionToCreate, ResourceLinks.SavedSelection, cancel);

        return createdSelection;
    }

    /// <summary>
    /// Download a selection.
    /// </summary>
    /// <param name="selection">selection to download.</param>
    /// <returns>dataseries.</returns>
    public async Task<Assertion<List<DataseriesDto>>> DownloadAsync(SelectionDto selection, CancellationToken cancel, int? page = null, int? pagesize = null)
    {
        Trace.WriteLine("DownloadAsync(" + selection + ")");

        string url;
        if (page.HasValue && pagesize.HasValue)
        {
            url = string.Format("{0}?page={1}&pagesize={2}", ResourceLinks.DownloadPath, page, pagesize);
        }
        else
        {
            url = ResourceLinks.DownloadPath;
        }

        Assertion<List<DataseriesDto>> data = await this.PerformPost<List<DataseriesDto>>(selection, url, cancel);

        return data;
    }

    /// <summary>
    /// Download a shaped selection.
    /// </summary>
    /// <param name="selection">selection to download.</param>
    /// <returns>dataseries.</returns>
    public async Task<Assertion<ShapedStreamResult>> DownloadShapedAsync(SelectionDto selection, ShapeConfigurationDto config, CancellationToken cancel = default(CancellationToken))
    {
        // Trace.WriteLine("DownloadShapedAsync(" + selection.Id + ")");
        Assertion<SelectionDto> selectionResponse = await this.GetConcreteSelection(selection, cancel);

        if (selectionResponse.Failed)
        {
            return Assertion<ShapedStreamResult>.Fail(selectionResponse);
        }

        // TODO: provide shapedDownload URL in
        string url = ResourceLinks.ShapedDownloadPath + "/" + selectionResponse.Result.Id;

        Assertion<ShapeCellDto[,]> data = await this.PerformPost<ShapeCellDto[,]>(config, url, cancel);

        if (data.Failed)
        {
            return Assertion<ShapedStreamResult>.Fail(data);
        }

        return Assertion<ShapedStreamResult>.Pass(new ShapedStreamResult
        {
            RowCount = data.Result.GetLength(0),
            ColumnCount = data.Result.GetLength(1),
            Rows = ArrayExtensions.ToJagged(data.Result),
        });
    }

    /// <summary>
    /// Download shaped streaming.
    /// </summary>
    /// <param name="selection">selection to download.</param>
    /// <returns>dataseries.</returns>
    public async Task<Assertion<ShapedStreamResult>> DownloadShapedStreamAsync(SelectionDto selection, ShapeConfigurationDto config, CancellationToken cancel = default(CancellationToken))
    {
        Trace.WriteLine("DownloadShapedStreamAsync(" + selection.Id + ")");

        ShapedDownloadRequest shapedDownloadRequest = new ShapedDownloadRequest
        {
            Selection = selection,
            Config = config,
        };

        MandolineWebRequest request = this._requestBuilder.CreateRequest(ResourceLinks.ShapedDownloadStreamingPath);

        Assertion<MandolineWebRequest.StreamResult<ShapedStreamResult, IEnumerable<ShapeCellDto>>> resultAssertion = await request.ExecuteStream<ShapedStreamResult, IEnumerable<ShapeCellDto>>(shapedDownloadRequest, "POST", cancel, false);

        if (!resultAssertion.Passed)
        {
            return Assertion<ShapedStreamResult>.Fail(resultAssertion);
        }

        ShapedStreamResult result = resultAssertion.Result.FirstObject;
        result.Rows = resultAssertion.Result.Remaining;

        return Assertion<ShapedStreamResult>.Pass(result);
    }

    public async Task<Assertion<string>> DownloadFileAsync(ControllerDownloadRequestDto request, CancellationToken token)
    {
        // TODO: provide download URL in DashboardWidgetDto
        string url = ResourceLinks.FileDownloadPath;
        Assertion<string> data = await this.PerformPost<string>(request, url, token, true);

        return data;
    }

    public async Task<Assertion<ControllerDownloadResponseDto>> RequestDownloadAsync(
        SelectionDto selection,
        FileFormat format,
        string name,
        CancellationToken cancel = default(CancellationToken))
    {
        Trace.WriteLine("RequestDownloadAsync");

        ControllerDownloadRequestDto controllerDownloadRequestDto = new ControllerDownloadRequestDto
        {
            selections = new[] { selection },
            format = format,
            name = name,
        };

        MandolineWebRequest request = this._requestBuilder.CreateRequest(ResourceLinks.DownloadRenderPath);

        Assertion<ControllerDownloadResponseDto> resultAssertion = await request.Execute<ControllerDownloadResponseDto>(controllerDownloadRequestDto, "POST", cancel);

        return resultAssertion;
    }

    public async Task<Assertion<string>> DownloadReadyAsync(
        ControllerDownloadResponseDto downloadResponse,
        CancellationToken cancel = default(CancellationToken))
    {
        MandolineWebRequest request = this._requestBuilder.CreateRequest(downloadResponse.ReadyUrl);

        return await request.Execute<string>(null, "GET", cancel);
    }

    public async Task<Assertion<RegionCollectionDto>> GetRegionsAsync(string databankCode)
    {
        Assertion<RegionCollectionDto> data = await this.PerformGet<RegionCollectionDto>(ResourceLinks.RegionsPath + "/" + databankCode);

        if (data.Failed)
        {
            Trace.WriteLine("GetRegionsAsync failed: " + data.Reason);
        }

        return data;
    }

    public async Task<Assertion<VariableCollectionDto>> GetVariablesAsync(string databankCode)
    {
        Assertion<VariableCollectionDto> data = await this.PerformGet<VariableCollectionDto>(ResourceLinks.VariablesPath + "/" + databankCode);

        if (data.Failed)
        {
            Trace.WriteLine("GetVariablesAsync failed: " + data.Reason);
        }

        return data;
    }

    public async Task<Assertion<Selection>> GetSavedSelection(Guid savedSelectionId)
    {
        // TODO: provide SelectionUrl server side
        string url = ResourceLinks.SavedSelection + "/" + savedSelectionId;

        Assertion<Selection> data = await this.PerformGet<Selection>(url);

        return data;
    }

    public async Task<Assertion<SelectionDto>> UpdateSavedSelectionAsync(Guid savedSelectionId, SelectionDto selection, CancellationToken cancel)
    {
        string url = ResourceLinks.SavedSelection + "/" + savedSelectionId;

        Assertion<SelectionDto> data = await this.PerformPut<SelectionDto>(selection, url, cancel);

        return data;
    }

    public async Task<Assertion<T>> PerformGet<T>(string url, CancellationToken cancel = default(CancellationToken))
        where T : class
    {
        Guard.NotNullOrEmpty(() => url, url);

        MandolineWebRequest request = this._requestBuilder.CreateRequest(url);
        return await request.Execute<T>(null, "GET", cancel);
    }

    /// <summary>
    /// Get an existing selection.
    /// </summary>
    /// <param name="selection">selection to download.</param>
    /// <returns>dataseries.</returns>
    private async Task<Assertion<SelectionDto>> GetConcreteSelection(SelectionDto selection, CancellationToken cancel)
    {
        // selection is already concrete, just return it
        if (!string.IsNullOrWhiteSpace(selection.DownloadUrl))
        {
            return Assertion<SelectionDto>.Pass(selection);
        }

        selection.IsTemporarySelection = true;
        Assertion<SelectionDto> result = await this.CreateSavedSelectionAsync(selection, cancel);
        return result;
    }

    private async Task<Assertion<T>> PerformPost<T>(object parameter, string url, CancellationToken cancel = default(CancellationToken), bool allowAutoRedirect = false)
        where T : class
    {
        Guard.NotNullOrEmpty(() => url, url);

        MandolineWebRequest request = this._requestBuilder.CreateRequest(url);
        return await request.Execute<T>(parameter, "POST", cancel);
    }

    private async Task<Assertion<T>> PerformPut<T>(object parameter, string url, CancellationToken cancel = default(CancellationToken))
        where T : class
    {
        Guard.NotNullOrEmpty(() => url, url);

        MandolineWebRequest request = this._requestBuilder.CreateRequest(url);
        return await request.Execute<T>(parameter, "PUT", cancel);
    }
}
