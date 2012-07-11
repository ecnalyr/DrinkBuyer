// -----------------------------------------------------------------------
// <copyright file="TextLineValidatorAtribute.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace DrinkBuyer.Domain.Validators
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Properties;

    #endregion

    /// <summary>
    ///   Only alpha-numeric characters and [.,_-;] are allowed.
    /// </summary>
    public class TextLineInputValidatorAtribute : RegularExpressionAttribute, IClientValidatable
    {
        #region Constructors and Destructors

        public TextLineInputValidatorAtribute()
            : base(Resources.TextLineInputValidatorRegEx)
        {
            this.ErrorMessage = Resources.InvalidInputCharacter;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
                {
                   ErrorMessage = Resources.InvalidInputCharacter, ValidationType = "textlineinput" 
                };
            rule.ValidationParameters.Add("pattern", Resources.TextLineInputValidatorRegEx);
            return new List<ModelClientValidationRule> { rule };
        }

        #endregion
    }
}