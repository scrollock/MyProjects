using System.Configuration;

namespace SupportIndeed.Configuration
{
    public class EmploeesCollection : ConfigurationElementCollection
    {
        public EmploeesConfigurationElement this[int index]
        {
            get { return BaseGet(index) as EmploeesConfigurationElement; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EmploeesConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmploeesConfigurationElement)element).id;
        }
    }
}