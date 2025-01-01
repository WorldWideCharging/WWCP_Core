﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#region Usings

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The common interface of all WWCP point-of-interest data management.
    /// </summary>
    public interface IReceiveChargingStationData
    {

        #region AddChargingStation           (ChargingStation,  ...)

        /// <summary>
        /// Add the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationResult>

            AddChargingStation(IChargingStation   ChargingStation,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default);

        #endregion

        #region AddChargingStationIfNotExists(ChargingStation,  ...)

        /// <summary>
        /// Add the given charging station, if it does not already exist.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add, if it does not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationResult>

            AddChargingStationIfNotExists(IChargingStation   ChargingStation,

                                          DateTime?          Timestamp           = null,
                                          EventTracking_Id?  EventTrackingId     = null,
                                          TimeSpan?          RequestTimeout      = null,
                                          CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStation   (ChargingStation,  ...)

        /// <summary>
        /// Add or update the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationResult>

            AddOrUpdateChargingStation(IChargingStation   ChargingStation,

                                       DateTime?          Timestamp           = null,
                                       EventTracking_Id?  EventTrackingId     = null,
                                       TimeSpan?          RequestTimeout      = null,
                                       CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateChargingStation        (ChargingStation,  PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Update the given charging station.
        /// The charging station can be uploaded as a whole, or just a single property of the charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to update.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationResult>

            UpdateChargingStation(IChargingStation   ChargingStation,
                                  String             PropertyName,
                                  Object?            NewValue,
                                  Object?            OldValue            = null,
                                  Context?           DataSource          = null,

                                  DateTime?          Timestamp           = null,
                                  EventTracking_Id?  EventTrackingId     = null,
                                  TimeSpan?          RequestTimeout      = null,
                                  CancellationToken  CancellationToken   = default);

        #endregion

        #region DeleteChargingStation        (ChargingStation,  ...)

        /// <summary>
        /// Delete the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationResult>

            DeleteChargingStation(IChargingStation   ChargingStation,

                                  DateTime?          Timestamp           = null,
                                  EventTracking_Id?  EventTrackingId     = null,
                                  TimeSpan?          RequestTimeout      = null,
                                  CancellationToken  CancellationToken   = default);

        #endregion


        #region AddChargingStations          (ChargingStations, ...)

        /// <summary>
        /// Add the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationsResult>

            AddChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                DateTime?                      Timestamp           = null,
                                EventTracking_Id?              EventTrackingId     = null,
                                TimeSpan?                      RequestTimeout      = null,
                                CancellationToken              CancellationToken   = default);

        #endregion

        #region AddChargingStationsIfNotExist(ChargingStations, ...)

        /// <summary>
        /// Add the given enumeration of charging stations, if they do not already exist..
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add, if they do not already exist..</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationsResult>

            AddChargingStationsIfNotExist(IEnumerable<IChargingStation>  ChargingStations,

                                          DateTime?                      Timestamp           = null,
                                          EventTracking_Id?              EventTrackingId     = null,
                                          TimeSpan?                      RequestTimeout      = null,
                                          CancellationToken              CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStations  (ChargingStations, ...)

        /// <summary>
        /// Add or update the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationsResult>

            AddOrUpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                        DateTime?                      Timestamp           = null,
                                        EventTracking_Id?              EventTrackingId     = null,
                                        TimeSpan?                      RequestTimeout      = null,
                                        CancellationToken              CancellationToken   = default);

        #endregion

        #region UpdateChargingStations       (ChargingStations, ...)

        /// <summary>
        /// Update the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationsResult>

            UpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                   DateTime?                      Timestamp           = null,
                                   EventTracking_Id?              EventTrackingId     = null,
                                   TimeSpan?                      RequestTimeout      = null,
                                   CancellationToken              CancellationToken   = default);

        #endregion

        #region DeleteChargingStations       (ChargingStations, ...)

        /// <summary>
        /// Delete the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationsResult>

            DeleteChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                   DateTime?                      Timestamp           = null,
                                   EventTracking_Id?              EventTrackingId     = null,
                                   TimeSpan?                      RequestTimeout      = null,
                                   CancellationToken              CancellationToken   = default);

        #endregion


    }

}
