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


namespace LL.Framework.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;
    using Expression_ = System.Linq.Expressions.Expression;

    using LL.Framework.Core.Domain;

    /// <summary>
    /// A logic AND Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class AndSpecification<T>
       : CompositeSpecification<T>
       where T : class
    {
        #region Members

        private Specification<T> _RightSideSpecification = null;
        private Specification<T> _LeftSideSpecification = null;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Default constructor for AndSpecification
        /// </summary>
        public AndSpecification(Specification<T> leftSide, Specification<T> rightSide)
        {
            this._LeftSideSpecification = leftSide ?? Specification<T>.True;
            this._RightSideSpecification = rightSide ?? Specification<T>.True;
        }

        #endregion

        #region Composite Specification overrides

        /// <summary>
        /// Left side specification
        /// </summary>
        public override Specification<T> LeftSideSpecification
        {
            get { return _LeftSideSpecification; }
        }

        /// <summary>
        /// Right side specification
        /// </summary>
        public override Specification<T> RightSideSpecification
        {
            get { return _RightSideSpecification; }
        }

        protected override BinaryExpression CreateBody(Expression left, Expression right)
        {
            return Expression_.AndAlso(left, right);
        }
        #endregion
    }
}
