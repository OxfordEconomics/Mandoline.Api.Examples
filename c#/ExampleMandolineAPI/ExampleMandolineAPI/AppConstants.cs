using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client.ServiceModels;
using System.Configuration;

namespace ExampleMandolineAPI
{
    public static class AppConstants 
    {
        // user's api token - default must be set before compile
        private static string _API_TOKEN { get; set; }
        public static string API_TOKEN
        {
            get
            {
                if (_API_TOKEN == null) _API_TOKEN = ConfigurationManager.AppSettings["API_TOKEN"] ?? "API_TOKEN not found"; // set this in AppSettings.config
                return _API_TOKEN;
            }
            set
            {
                if (value != null && value != "") _API_TOKEN = value;
            }
        }

        // id that examples will use in pulling sample selections
        public static string SAVED_SELECTION_ID { get { return ConfigurationManager.AppSettings["SELECTION_ID"] ?? "SELECTION_ID not found";  } }

        // simple example selection: draws GDP and inflation data from the US, the UK, France, and Germany 
        public class SampleSelect : SelectionDto {
            private static SelectionDto instance = null;
            private SampleSelect() { }
            public static SelectionDto GetInstance()
            {
                if(instance == null)
                {
                    instance = new SelectionDto()
                    {
                        Name = "Selection - Created: " + DateTime.Now,
                        DatabankCode = "WDMacro",
                        MeasureCode = "L",
                        StartYear = 2015,
                        EndYear = 2021,
                        StackedQuarters = false,
                        Frequency = FrequencyEnum.Annual,
                        Sequence = SequenceEnum.EarliestToLatest,
                        Precision = 1,
                        Order = OrderEnum.IndicatorLocation,
                        GroupingMode = false,
                        SelectionType = SelectionTypeEnum.DataseriesSelection,
                        IsTemporarySelection = false,
                        ListingType = ListingTypeEnum.Private,
                        Regions = new List<SelectionRegionDto>
                        {
                            new SelectionRegionDto
                            {
                                DatabankCode = "WDMacro",
                                RegionCode = "GBR",
                            },

                            new SelectionRegionDto
                            {
                                DatabankCode = "WDMacro",
                                RegionCode = "USA",
                            },

                            new SelectionRegionDto
                            {
                                DatabankCode = "WDMacro",
                                RegionCode = "FRA",
                            },

                            new SelectionRegionDto
                            {
                                DatabankCode = "WDMacro",
                                RegionCode = "DEU",
                            }

                        },
                        Variables = new List<SelectionVariableDto>
                        {
                            new SelectionVariableDto
                            {
                                ProductTypeCode = "WMC",
                                VariableCode = "GDP$",
                                MeasureCodes = new string[] { "L" ,"PY","DY" }
                            },
                            new SelectionVariableDto
                            {
                                ProductTypeCode = "WMC",
                                VariableCode = "CPI",
                                MeasureCodes = new string[] {"L"}
                            }
                        }

                    };

                }

                return instance;
            }

        }


    }
}
