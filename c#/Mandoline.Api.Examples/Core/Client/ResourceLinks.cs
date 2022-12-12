namespace Core.Client;

public class ResourceLinks
{
#if DEBUG
    // public const string APIURL = "http://localhost:59768/";
    // public const string APIURL = "http://192.168.0.233/Mandoline.Web/";
    // public const string APIURL = "http://staging.oxfordeconomics.com/Rotation1/";
    public const string APIURL = "http://services.oxfordeconomics.com/";

    // public const string APIURL = "http://oeox1vw-dap-001/excel/";
    // public const string APIURL = "http://stevendawkins/data/";
    // public const string APIURL = "http://oeox1vw-dap-001/API/";
    public const string LOCALPIPE = "Julienne";

#else
		// Note that the excel plugin no longer reads this
    public const string APIURL = "http://services.oxfordeconomics.com/";
    public const string LOCALPIPE = "Julienne";
    // public const string APIURL = "http://oeox1vw-dap-001/excel/";
    // public const string APIURL = "http://staging.oxfordeconomics.com/Rotation1/";
#endif

    public const string BasePath = "/API/";

    public const string UserPath = BasePath + "users";
    public const string SavedSelection = BasePath + "savedselections";
    public const string DatabankPath = BasePath + "databank";
    public const string WidgetsPath = BasePath + "dashboardwidget";
    public const string MapPath = BasePath + "map";
    public const string RegionsPath = BasePath + "region";
    public const string VariablesPath = BasePath + "variable";

    // TODO: remove, should be implicit from hypermedia
    public const string DownloadPath = BasePath + "download";
    public const string FileDownloadPath = BasePath + "filedownload";

    public const string ShapedDownloadPath = BasePath + "shapeddownload";
    public const string ShapedDownloadStreamingPath = BasePath + "shapeddownloadstreaming";
    public const string DownloadRenderPath = BasePath + "QueueDownload";
}
