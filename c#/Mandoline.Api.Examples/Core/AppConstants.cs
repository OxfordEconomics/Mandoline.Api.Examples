﻿// <copyright file="AppConstants.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mandoline.Api.Client.ServiceModels;

    public static class AppConstants
    {
        static AppConstants()
        {
            _ApiToken = null;
            _SavedSelectionId = Guid.Empty;
            BaseURL = ConfigurationManager.AppSettings["BASE_URL"];
            UserName = ConfigurationManager.AppSettings["USER_NAME"];
            UserPassword = ConfigurationManager.AppSettings["USER_PASS"];
        }

        // user's api token - default must be set before compile
        public static string ApiToken
        {
            get
            {
                if (_ApiToken == null)
                {
                    _ApiToken = ConfigurationManager.AppSettings["API_TOKEN"] ?? null; // set this in AppSettings.config
                }

                return _ApiToken;
            }

            set
            {
                if (value != null && value != string.Empty)
                {
                    _ApiToken = value;
                }
            }
        }

        // only used in testing
        public static string UserName { get; set; }

        // only used in testing
        public static string UserPassword { get; set; }

        // id that examples will use in pulling sample selections
        public static Guid SavedSelectionId
        {
            get
            {
                if (_SavedSelectionId == Guid.Empty)
                {
                    string newId = ConfigurationManager.AppSettings["SELECTION_ID"];
                    _SavedSelectionId = (newId == string.Empty || newId == null) ? Guid.Empty : new Guid(newId); // set this in AppSettings.config
                }

                return _SavedSelectionId;
            }

            set
            {
                if (value != null)
                {
                    _SavedSelectionId = value;
                }
            }
        }

        // base url for api calls
        public static string BaseURL { get; set; }

        // simple example selection: draws GDP and inflation data from the US, the UK, France, and Germany
        public class SampleSelection : SelectionDto
        {
            private static SelectionDto instance = null;

            private SampleSelection()
            {
            }

            public static SelectionDto GetInstance()
            {
                if (instance == null)
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
                                MeasureCodes = new string[] { "L", "PY", "DY" }
                            },
                            new SelectionVariableDto
                            {
                                ProductTypeCode = "WMC",
                                VariableCode = "CPI",
                                MeasureCodes = new string[]
                                {
                                    "L"
                                }
                            }
                        }
                    };
                }

                return instance;
            }
        }

        // private members corresponding to public variables
        private static string _ApiToken { get; set; }

        private static Guid _SavedSelectionId { get; set; }
    }
}
