﻿/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushChargingStationAdminStatus result.
    /// </summary>
    public class PushChargingStationAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                            AuthId                                        { get; }

        /// <summary>
        /// An object implementing ISendAdminStatus.
        /// </summary>
        public IPushAdminStatus?                              ISendAdminStatus                              { get; }

        /// <summary>
        /// An object implementing IReceiveAdminStatus.
        /// </summary>
        public IReceiveAdminStatus?                           IReceiveAdminStatus                           { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingStationAdminStatusResultTypes      Result                                        { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                        Description                                   { get; }

        /// <summary>
        /// An enumeration of rejected charging station admin status updates.
        /// </summary>
        public IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                           Warnings                                      { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                      Runtime                                       { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushChargingStationAdminStatusResult(AuthId,                      Result, ...)

        /// <summary>
        /// Create a new PushChargingStationAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationAdminStatusUpdates">An enumeration of rejected charging station admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationAdminStatusResult(IId                                             AuthId,
                                                     PushChargingStationAdminStatusResultTypes       Result,
                                                     String?                                         Description                                 = null,
                                                     IEnumerable<ChargingStationAdminStatusUpdate>?  RejectedChargingStationAdminStatusUpdates   = null,
                                                     IEnumerable<Warning>?                           Warnings                                    = null,
                                                     TimeSpan?                                       Runtime                                     = null)
        {

            this.AuthId                                     = AuthId;
            this.Result                                     = Result;

            this.Description                                = Description is not null && Description.IsNotNullOrEmpty()
                                                                  ? Description.Trim()
                                                                  : String.Empty;

            this.RejectedChargingStationAdminStatusUpdates  = RejectedChargingStationAdminStatusUpdates ?? Array.Empty<ChargingStationAdminStatusUpdate>();

            this.Warnings                                   = Warnings is not null
                                                                  ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                  : Array.Empty<Warning>();

            this.Runtime                                    = Runtime;

        }

        #endregion

        #region (internal) PushChargingStationAdminStatusResult(AuthId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationAdminStatusUpdates">An enumeration of rejected charging station admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationAdminStatusResult(IId                                             AuthId,
                                                      IPushAdminStatus                                ISendAdminStatus,
                                                      PushChargingStationAdminStatusResultTypes       Result,
                                                      String?                                         Description                                 = null,
                                                      IEnumerable<ChargingStationAdminStatusUpdate>?  RejectedChargingStationAdminStatusUpdates   = null,
                                                      IEnumerable<Warning>?                           Warnings                                    = null,
                                                      TimeSpan?                                       Runtime                                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushChargingStationAdminStatusResult(AuthId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationAdminStatusUpdates">An enumeration of rejected charging station admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationAdminStatusResult(IId                                             AuthId,
                                                      IReceiveAdminStatus                             IReceiveAdminStatus,
                                                      PushChargingStationAdminStatusResultTypes       Result,
                                                      String?                                         Description                                 = null,
                                                      IEnumerable<ChargingStationAdminStatusUpdate>?  RejectedChargingStationAdminStatusUpdates   = null,
                                                      IEnumerable<Warning>?                           Warnings                                    = null,
                                                      TimeSpan?                                       Runtime                                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingStationAdminStatusResult

            Success(IId                    AuthId,
                    IPushAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingStationAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationAdminStatusResult

            Success(IId                    AuthId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingStationAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationAdminStatusResult

            Enqueued(IId                    AuthId,
                     IPushAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingStationAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingStationAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationAdminStatusResult

            NoOperation(IId                    AuthId,
                        IPushAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingStationAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationAdminStatusResult

            NoOperation(IId                    AuthId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingStationAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationAdminStatusResult

            OutOfService(IId                                                    AuthId,
                         IPushAdminStatus                                       ISendAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingStationAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationAdminStatusResult

            OutOfService(IId                                                    AuthId,
                         IReceiveAdminStatus                                    IReceiveAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingStationAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationAdminStatusResult

            AdminDown(IId                                                    AuthId,
                      IPushAdminStatus                                       ISendAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingStationAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationAdminStatusResult

            AdminDown(IId                                                    AuthId,
                      IReceiveAdminStatus                                    IReceiveAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingStationAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushChargingStationAdminStatusResult

            Error(IId                                             AuthId,
                  IPushAdminStatus                                ISendAdminStatus,
                  IEnumerable<ChargingStationAdminStatusUpdate>?  RejectedChargingStationAdminStatusUpdates   = null,
                  String?                                         Description                                 = null,
                  IEnumerable<Warning>?                           Warnings                                    = null,
                  TimeSpan?                                       Runtime                                     = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingStationAdminStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationAdminStatusResult

            Error(IId                                             AuthId,
                  IReceiveAdminStatus                             IReceiveAdminStatus,
                  IEnumerable<ChargingStationAdminStatusUpdate>?  RejectedChargingStationAdminStatusUpdates   = null,
                  String?                                         Description                                 = null,
                  IEnumerable<Warning>?                           Warnings                                    = null,
                  TimeSpan?                                       Runtime                                     = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingStationAdminStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationAdminStatusResult

            LockTimeout(IId                                            AuthId,
                        IPushAdminStatus                               ISendAdminStatus,
                        IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                        String?                                        Description   = null,
                        IEnumerable<Warning>?                          Warnings      = null,
                        TimeSpan?                                      Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingStationAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedChargingStationAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(AuthId, ISendAdminStatus, PushChargingStationAdminStatusResults, Runtime)

        public static PushChargingStationAdminStatusResult Flatten(IId                                                AuthId,
                                                                   IPushAdminStatus                                   ISendAdminStatus,
                                                                   IEnumerable<PushChargingStationAdminStatusResult>  PushChargingStationAdminStatusResults,
                                                                   TimeSpan                                           Runtime)
        {

            #region Initial checks

            if (PushChargingStationAdminStatusResults is null || !PushChargingStationAdminStatusResults.Any())
                return new PushChargingStationAdminStatusResult(AuthId,
                                                                ISendAdminStatus,
                                                                PushChargingStationAdminStatusResultTypes.Error,
                                                                "!",
                                                                Array.Empty<ChargingStationAdminStatusUpdate>(),
                                                                Array.Empty<Warning>(),
                                                                Runtime);

            #endregion

            var all                                        = PushChargingStationAdminStatusResults.ToArray();

            var resultOverview                             = all.GroupBy      (result => result.Result).
                                                                 ToDictionary (result => result.Key,
                                                                               result => new List<PushChargingStationAdminStatusResult>(result));

            var descriptions                               = all.Where        (result => result is not null).
                                                                 SafeSelect   (result => result.Description).
                                                                 AggregateWith(Environment.NewLine);

            var rejectedChargingStationAdminStatusUpdates  = all.Where        (result => result is not null).
                                                                 SelectMany   (result => result.RejectedChargingStationAdminStatusUpdates);

            var warnings                                   = all.Where        (result => result is not null).
                                                                 SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushChargingStationAdminStatusResult(all[0].AuthId,
                                                                    ISendAdminStatus,
                                                                    result.Key,
                                                                    descriptions,
                                                                    rejectedChargingStationAdminStatusUpdates,
                                                                    warnings,
                                                                    Runtime);

            return new PushChargingStationAdminStatusResult(all[0].AuthId,
                                                            ISendAdminStatus,
                                                            PushChargingStationAdminStatusResultTypes.Partial,
                                                            descriptions,
                                                            rejectedChargingStationAdminStatusUpdates,
                                                            warnings,
                                                            Runtime);

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public enum PushChargingStationAdminStatusResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        Success,
        Partial,
        OutOfService,
        Error,
        True,
        NoOperation,
        Enqueued,
        LockTimeout,
        False

    }

}
