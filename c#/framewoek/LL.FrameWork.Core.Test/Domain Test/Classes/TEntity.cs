//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


namespace Domain.Seedwork.Tests.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LL.FrameWork.Core.Domain;

    public class SampleEntity : EntityBase<Guid>
    {
        public string SampleProperty { get; set; }
    }
}
