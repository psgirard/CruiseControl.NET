namespace ThoughtWorks.CruiseControl.Core.Tasks.Conditions
{
   using System;
   using System.Collections.Generic;
   using Exortech.NetReflector;
    using ThoughtWorks.CruiseControl.Core.Tasks;
    using ThoughtWorks.CruiseControl.Core.Util;
   using ThoughtWorks.CruiseControl.Remote;
   using ThoughtWorks.CruiseControl.Remote.Parameters;

    /// <summary>
    /// Provides a base implementation for conditions that provides some common functionality.
    /// </summary>
    public abstract class ConditionBase
        : IParamatisedItem, ITaskCondition
    {
       #region Protected fields
       protected Dictionary<string, string> parameters;
       protected IEnumerable<ParameterBase> parameterDefinitions;
       protected IDynamicValue[] myDynamicValues = new IDynamicValue[0];
       #endregion
       
       #region Public properties
       #region DynamicValues
       /// <summary>
       /// The dynamic values to use for the task.
       /// </summary>
       /// <version>1.5</version>
       /// <default>None</default>
       [ReflectorProperty("dynamicValues", Required = false)]
       public IDynamicValue[] DynamicValues
       {
          get { return myDynamicValues; }
          set { myDynamicValues = value; }
       }
       #endregion
       
       #region Description
        /// <summary>
        /// A description of the condition.
        /// </summary>
        /// <version>1.6</version>
        /// <default>none</default>
        [ReflectorProperty("description", Required = false)]
        public string Description { get; set; }
        #endregion

        #region Logger
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public ILogger Logger { get; set; }
        #endregion
        #endregion

        #region Public methods
        #region ApplyParameters()
        /// <summary>
        /// Applies the input parameters to the task.
        /// </summary>
        /// <param name="parameters">The parameters to apply.</param>
        /// <param name="parameterDefinitions">The original parameter definitions.</param>
        public virtual void ApplyParameters(Dictionary<string, string> parametersToApply, IEnumerable<ParameterBase> parameterDefinitionsToUse)
        {
           if (myDynamicValues != null)
           {
              this.parameters = parametersToApply;
              this.parameterDefinitions = parameterDefinitionsToUse;

              foreach (IDynamicValue value in myDynamicValues)
              {
                 value.ApplyTo(this, parametersToApply, parameterDefinitionsToUse);
              }
           }
        }
        #endregion

        #region Eval()
        /// <summary>
        /// Evals the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>
        /// <c>true</c> if the condition is true; <c>false</c> otherwise.
        /// </returns>
        public virtual bool Eval(IIntegrationResult result)
        {
            var evaluation = this.Evaluate(result);
            return evaluation;
        }
        #endregion
        #endregion

        #region Protected methods
        #region Evaluate()
        /// <summary>
        /// Performs the actual evaluation.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>
        /// <c>true</c> if the condition is true; <c>false</c> otherwise.
        /// </returns>
        protected abstract bool Evaluate(IIntegrationResult result);
        #endregion

        #region RetrieveLogger()
        /// <summary>
        /// Retrieves the logger instance.
        /// </summary>
        /// <returns>The <see cref="ILogger"/> to use.</returns>
        protected ILogger RetrieveLogger()
        {
            return this.Logger ?? (this.Logger = new DefaultLogger());
        }

        #endregion

        #region LogDescriptionOrMessage()
        /// <summary>
        /// Logs the description or a message.
        /// </summary>
        /// <param name="message">The message to use if there is no description.</param>
        protected void LogDescriptionOrMessage(string message)
        {
            var logger = this.RetrieveLogger();
            if (string.IsNullOrEmpty(this.Description))
            {
                logger.Debug(message);
            }
            else
            {
                logger.Info(this.Description);
            }
        }
        #endregion
        #endregion

        internal void ApplyParameters(List<NameValuePair> listParams, IEnumerable<ParameterBase> parameterDefinitions)
        {
           Dictionary<string, string> parameters = new Dictionary<string, string>();
           foreach (var item in listParams)
           {
              parameters.Add(item.Name, item.Value);
           }

           this.ApplyParameters(parameters, parameterDefinitions);
        }
    }
}
