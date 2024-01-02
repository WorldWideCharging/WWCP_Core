﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// Extension methods for EVSE admin status reports.
    /// </summary>
    public static class EVSEAdminStatusReportExtensions
    {

        #region GenerateAdminStatusReport(this EVSE,                     Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public static EVSEAdminStatusReport GenerateAdminStatusReport(this IEVSE                                      EVSE,
                                                                      DateTime?                                       Timestamp   = null)

            => new (new IEVSE[] { EVSE },
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this EVSEs,                    Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        public static EVSEAdminStatusReport GenerateAdminStatusReport(this IEnumerable<IEVSE>                         EVSEs,
                                                                      DateTime?                                       Timestamp   = null)

            => new (EVSEs,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingStation,          Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static EVSEAdminStatusReport GenerateEVSEAdminStatusReport(this IChargingStation                       ChargingStation,
                                                                          DateTime?                                   Timestamp   = null)

            => new (ChargingStation.EVSEs,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingStations,         Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        public static EVSEAdminStatusReport GenerateEVSEAdminStatusReport(this IEnumerable<IChargingStation>          ChargingStations,
                                                                          DateTime?                                   Timestamp   = null)

            => new (ChargingStations.SelectMany(chargingStation => chargingStation.EVSEs),
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingPool,             Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static EVSEAdminStatusReport GenerateEVSEAdminStatusReport(this IChargingPool                          ChargingPool,
                                                                          DateTime?                                   Timestamp   = null)

            => new (ChargingPool.EVSEs,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingPools,            Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        public static EVSEAdminStatusReport GenerateEVSEAdminStatusReport(this IEnumerable<IChargingPool>             ChargingPools,
                                                                          DateTime?                                   Timestamp   = null)

            => new (ChargingPools.SelectMany(chargingPool => chargingPool.EVSEs),
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingStationOperator,  Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static EVSEAdminStatusReport GenerateEVSEAdminStatusReport(this IChargingStationOperator               ChargingStationOperator,
                                                                          DateTime?                                   Timestamp   = null)

            => new (ChargingStationOperator.EVSEs,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingStationOperators, Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        public static EVSEAdminStatusReport GenerateEVSEAdminStatusReport(this IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                          DateTime?                                   Timestamp   = null)

            => new (ChargingStationOperators.SelectMany(chargingStationOperator => chargingStationOperator.EVSEs),
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this RoamingNetwork,           Timestamp = null)

        /// <summary>
        /// Generate a new EVSE admin status report for the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static EVSEAdminStatusReport GenerateEVSEAdminStatusReport(this IRoamingNetwork                        RoamingNetwork,
                                                                          DateTime?                                   Timestamp   = null)

            => new (RoamingNetwork.EVSEs,
                    Timestamp);

        #endregion

    }


    /// <summary>
    /// An EVSE admin status report.
    /// </summary>
    public class EVSEAdminStatusReport : StatusReport<IEVSE, EVSEAdminStatusTypes>
    {

        /// <summary>
        /// Create a new EVSE admin status report for the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public EVSEAdminStatusReport(IEnumerable<IEVSE>  EVSEs,
                                     DateTime?           Timestamp = null)

            : base(EVSEs,
                   evse => evse.AdminStatus.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/evseAdminStatusReport")

        { }

    }

}
