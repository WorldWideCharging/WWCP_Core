﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for EVSE status reports.
    /// </summary>
    public static class EVSEStatusReportExtensions
    {

        /// <summary>
        /// Generate a new EVSE status report for the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public static EVSEStatusReport GenerateStatusReport(this EVSE                                      EVSE,
                                                            DateTime?                                      Timestamp = null)

            => new (new EVSE[] { EVSE },
                    Timestamp);

        /// <summary>
        /// Generate a new EVSE status report for the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        public static EVSEStatusReport GenerateStatusReport(this IEnumerable<EVSE>                         EVSEs,
                                                            DateTime?                                      Timestamp = null)

            => new (EVSEs,
                    Timestamp);


        /// <summary>
        /// Generate a new EVSE status report for the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static EVSEStatusReport GenerateEVSEStatusReport(this ChargingStation                       ChargingStation,
                                                                DateTime?                                  Timestamp = null)

            => new (ChargingStation.EVSEs,
                    Timestamp);

        /// <summary>
        /// Generate a new EVSE status report for the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        public static EVSEStatusReport GenerateEVSEStatusReport(this IEnumerable<ChargingStation>          ChargingStations,
                                                                DateTime?                                  Timestamp = null)

            => new (ChargingStations.SelectMany(chargingStation => chargingStation.EVSEs),
                    Timestamp);

        /// <summary>
        /// Generate a new EVSE status report for the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static EVSEStatusReport GenerateEVSEStatusReport(this ChargingPool                          ChargingPool,
                                                                DateTime?                                  Timestamp = null)

            => new (ChargingPool.EVSEs,
                    Timestamp);

        /// <summary>
        /// Generate a new EVSE status report for the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        public static EVSEStatusReport GenerateEVSEStatusReport(this IEnumerable<ChargingPool>             ChargingPools,
                                                                DateTime?                                  Timestamp = null)

            => new (ChargingPools.SelectMany(chargingPool => chargingPool.EVSEs),
                    Timestamp);

        /// <summary>
        /// Generate a new EVSE status report for the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static EVSEStatusReport GenerateEVSEStatusReport(this ChargingStationOperator               ChargingStationOperator,
                                                                DateTime?                                  Timestamp = null)

            => new (ChargingStationOperator.EVSEs,
                    Timestamp);

        /// <summary>
        /// Generate a new EVSE status report for the given charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        public static EVSEStatusReport GenerateEVSEStatusReport(this IEnumerable<ChargingStationOperator>  ChargingStationOperators,
                                                                DateTime?                                  Timestamp = null)

            => new (ChargingStationOperators.SelectMany(chargingStationOperator => chargingStationOperator.EVSEs),
                    Timestamp);

        /// <summary>
        /// Generate a new EVSE status report for the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static EVSEStatusReport GenerateEVSEStatusReport(this RoamingNetwork                        RoamingNetwork,
                                                                DateTime?                                  Timestamp = null)

            => new (RoamingNetwork.EVSEs,
                    Timestamp);

    }


    /// <summary>
    /// An EVSE status report.
    /// </summary>
    public class EVSEStatusReport : StatusReport<IEVSE, EVSEStatusTypes>
    {

        /// <summary>
        /// Create a new EVSE status report for the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public EVSEStatusReport(IEnumerable<IEVSE>  EVSEs,
                                DateTime?           Timestamp = null)

            : base(EVSEs,
                   evse => evse.Status.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/evseStatusReport")

        { }

    }

}
