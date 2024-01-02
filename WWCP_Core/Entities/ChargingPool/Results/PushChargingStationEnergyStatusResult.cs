﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A PushChargingPoolEnergyStatus result.
    /// </summary>
    public class PushChargingPoolEnergyStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                             SenderId                                        { get; }

        /// <summary>
        /// An object implementing ISendEnergyStatus.
        /// </summary>
        public ISendEnergyStatus?                              ISendEnergyStatus                             { get; }

        /// <summary>
        /// An object implementing IReceiveEnergyStatus.
        /// </summary>
        public IReceiveEnergyStatus?                           IReceiveEnergyStatus                          { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingPoolEnergyStatusResultTypes      Result                                        { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                         Description                                   { get; }

        /// <summary>
        /// An enumeration of rejected ChargingPool status updates.
        /// </summary>
        public IEnumerable<ChargingPoolEnergyStatusUpdate>  RejectedChargingPoolEnergyStatusUpdates    { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                            Warnings                                      { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan                                        Runtime                                       { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushChargingPoolEnergyStatusResult(SenderId,                       Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolEnergyStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolEnergyStatusUpdates">An enumeration of rejected ChargingPool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingPoolEnergyStatusResult(IId                                           SenderId,
                                                   PushChargingPoolEnergyStatusResultTypes       Result,
                                                   String?                                       Description                               = null,
                                                   IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPoolEnergyStatusUpdates   = null,
                                                   IEnumerable<Warning>?                         Warnings                                  = null,
                                                   TimeSpan?                                     Runtime                                   = null)
        {

            this.SenderId                                 = SenderId;
            this.Result                                   = Result;
            this.Description                              = Description?.Trim();
            this.RejectedChargingPoolEnergyStatusUpdates  = RejectedChargingPoolEnergyStatusUpdates?.Distinct() ?? Array.Empty<ChargingPoolEnergyStatusUpdate>();
            this.Warnings                                 = Warnings?.                               Distinct() ?? Array.Empty<Warning>();
            this.Runtime                                  = Runtime                                             ?? TimeSpan.Zero;

        }

        #endregion

        #region (internal) PushChargingPoolEnergyStatusResult(SenderId, ISendEnergyStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolEnergyStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="ISendEnergyStatus">An object implementing ISendEnergyStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolEnergyStatusUpdates">An enumeration of rejected ChargingPool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolEnergyStatusResult(IId                                              SenderId,
                                                       ISendEnergyStatus                                ISendEnergyStatus,
                                                       PushChargingPoolEnergyStatusResultTypes       Result,
                                                       String?                                          Description                                  = null,
                                                       IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPoolEnergyStatusUpdates   = null,
                                                       IEnumerable<Warning>?                            Warnings                                     = null,
                                                       TimeSpan?                                        Runtime                                      = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedChargingPoolEnergyStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendEnergyStatus = ISendEnergyStatus;

        }

        #endregion

        #region (internal) PushChargingPoolEnergyStatusResult(SenderId, IReceiveEnergyStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolEnergyStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="IReceiveEnergyStatus">An object implementing IReceiveEnergyStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolEnergyStatusUpdates">An enumeration of rejected ChargingPool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolEnergyStatusResult(IId                                              SenderId,
                                                       IReceiveEnergyStatus                             IReceiveEnergyStatus,
                                                       PushChargingPoolEnergyStatusResultTypes       Result,
                                                       String?                                          Description                                  = null,
                                                       IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPoolEnergyStatusUpdates   = null,
                                                       IEnumerable<Warning>?                            Warnings                                     = null,
                                                       TimeSpan?                                        Runtime                                      = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedChargingPoolEnergyStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveEnergyStatus = IReceiveEnergyStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingPoolEnergyStatusResult

            Success(IId                    SenderId,
                    ISendEnergyStatus      ISendEnergyStatus,
                    String?                Description    = null,
                    IEnumerable<Warning>?  Warnings       = null,
                    TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingPoolEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);


        public static PushChargingPoolEnergyStatusResult

            Success(IId                    SenderId,
                    IReceiveEnergyStatus   IReceiveEnergyStatus,
                    String?                Description    = null,
                    IEnumerable<Warning>?  Warnings       = null,
                    TimeSpan?              Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingPoolEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingPoolEnergyStatusResult

            Enqueued(IId                    SenderId,
                     ISendEnergyStatus            ISendEnergyStatus,
                     String?                Description    = null,
                     IEnumerable<Warning>?  Warnings       = null,
                     TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingPoolEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingPoolEnergyStatusResult

            NoOperation(IId                             SenderId,
                        ISendEnergyStatus                     ISendEnergyStatus,
                        String?                         Description                 = null,
                        IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPoolEnergyStatusUpdates   = null,
                        IEnumerable<Warning>?           Warnings                    = null,
                        TimeSpan?                       Runtime                     = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.NoOperation,
                    Description,
                    RejectedChargingPoolEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingPoolEnergyStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveEnergyStatus         IReceiveEnergyStatus,
                        String?                Description    = null,
                        IEnumerable<Warning>?  Warnings       = null,
                        TimeSpan?              Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingPoolEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingPoolEnergyStatusResult

            OutOfService(IId                            SenderId,
                         ISendEnergyStatus                    ISendEnergyStatus,
                         IEnumerable<ChargingPoolEnergyStatusUpdate>  RejectedChargingPoolEnergyStatusUpdates,
                         String?                        Description    = null,
                         IEnumerable<Warning>?          Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingPoolEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingPoolEnergyStatusResult

            OutOfService(IId                            SenderId,
                         IReceiveEnergyStatus                 IReceiveEnergyStatus,
                         IEnumerable<ChargingPoolEnergyStatusUpdate>  RejectedChargingPoolEnergyStatusUpdates,
                         String?                        Description    = null,
                         IEnumerable<Warning>?          Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingPoolEnergyStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingPoolEnergyStatusResult

            AdminDown(IId                            SenderId,
                      ISendEnergyStatus                    ISendEnergyStatus,
                      IEnumerable<ChargingPoolEnergyStatusUpdate>  RejectedChargingPoolEnergyStatusUpdates,
                      String?                        Description   = null,
                      IEnumerable<Warning>?          Warnings      = null,
                      TimeSpan?                      Runtime       = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingPoolEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingPoolEnergyStatusResult

            AdminDown(IId                            SenderId,
                      IReceiveEnergyStatus                 IReceiveEnergyStatus,
                      IEnumerable<ChargingPoolEnergyStatusUpdate>  RejectedChargingPoolEnergyStatusUpdates,
                      String?                        Description   = null,
                      IEnumerable<Warning>?          Warnings      = null,
                      TimeSpan?                      Runtime       = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingPoolEnergyStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region Error

        public static PushChargingPoolEnergyStatusResult

            Error(IId                             SenderId,
                  ISendEnergyStatus                     ISendEnergyStatus,
                  IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPools   = null,
                  String?                         Description     = null,
                  IEnumerable<Warning>?           Warnings        = null,
                  TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.Error,
                    Description,
                    RejectedChargingPools,
                    Warnings,
                    Runtime);


        public static PushChargingPoolEnergyStatusResult

            Error(IId                             SenderId,
                  IReceiveEnergyStatus                  IReceiveEnergyStatus,
                  IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPools   = null,
                  String?                         Description     = null,
                  IEnumerable<Warning>?           Warnings        = null,
                  TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.Error,
                    Description,
                    RejectedChargingPools,
                    Warnings,
                    Runtime);

        #endregion

        #region Failed

        public static PushChargingPoolEnergyStatusResult

            Failed(IId                             SenderId,
                   ISendEnergyStatus                     ISendEnergyStatus,
                   IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPools   = null,
                   String?                         Description     = null,
                   IEnumerable<Warning>?           Warnings        = null,
                   TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.Error,
                    Description,
                    RejectedChargingPools,
                    Warnings,
                    Runtime);


        public static PushChargingPoolEnergyStatusResult

            Failed(IId                             SenderId,
                   IReceiveEnergyStatus                  IReceiveEnergyStatus,
                   IEnumerable<ChargingPoolEnergyStatusUpdate>?  RejectedChargingPools   = null,
                   String?                         Description     = null,
                   IEnumerable<Warning>?           Warnings        = null,
                   TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.Failed,
                    Description,
                    RejectedChargingPools,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingPoolEnergyStatusResult

            LockTimeout(IId                    SenderId,
                        ISendEnergyStatus            ISendEnergyStatus,
                        String?                Description    = null,
                        IEnumerable<Warning>?  Warnings       = null,
                        TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingPoolEnergyStatusResultTypes.LockTimeout,
                    Description,
                    Array.Empty<ChargingPoolEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Flatten(...)

        public static PushChargingPoolEnergyStatusResult Flatten(IId                                SenderId,
                                                   ISendEnergyStatus                        ISendEnergyStatus,
                                                   IEnumerable<PushChargingPoolEnergyStatusResult>  PushChargingPoolEnergyStatusResults,
                                                   TimeSpan                           Runtime)
        {

            #region Initial checks

            if (PushChargingPoolEnergyStatusResults == null || !PushChargingPoolEnergyStatusResults.Any())
                return new PushChargingPoolEnergyStatusResult(SenderId,
                                                ISendEnergyStatus,
                                                PushChargingPoolEnergyStatusResultTypes.Error,
                                                "!",
                                                new ChargingPoolEnergyStatusUpdate[0],
                                                new Warning[0],
                                                Runtime);

            #endregion

            var All                        = PushChargingPoolEnergyStatusResults.ToArray();

            var ResultOverview             = All.GroupBy     (_ => _.Result).
                                                 ToDictionary(_ => _.Key,
                                                              _ => new List<PushChargingPoolEnergyStatusResult>(_));

            var Descriptions               = All.Where       (_ => _ != null).
                                                 SafeSelect  (_ => _.Description).
                                                 AggregateWith(Environment.NewLine);

            var RejectedChargingPoolEnergyStatusUpdates  = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.RejectedChargingPoolEnergyStatusUpdates);

            var Warnings                   = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushChargingPoolEnergyStatusResult(All[0].SenderId,
                                                    ISendEnergyStatus,
                                                    result.Key,
                                                    Descriptions,
                                                    RejectedChargingPoolEnergyStatusUpdates,
                                                    Warnings,
                                                    Runtime);

            return new PushChargingPoolEnergyStatusResult(All[0].SenderId,
                                            ISendEnergyStatus,
                                            PushChargingPoolEnergyStatusResultTypes.Partial,
                                            Descriptions,
                                            RejectedChargingPoolEnergyStatusUpdates,
                                            Warnings,
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


    public enum PushChargingPoolEnergyStatusResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        OutOfService,

        Success,

        Partial,

        NoOperation,

        Enqueued,

        Error,

        Failed,

        LockTimeout

    }

}
