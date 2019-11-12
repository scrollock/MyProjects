using System.Configuration;

namespace SupportIndeed.Configuration
{
    public class EmploeesConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(level), IsRequired = true)]
        public string level => this[nameof(level)] as string;

        [ConfigurationProperty(nameof(position), IsRequired = true)]
        public string position => this[nameof(position)] as string;

        [ConfigurationProperty(nameof(name), IsRequired = true)]
        public string name => this[nameof(name)] as string;

        [ConfigurationProperty(nameof(secondName), IsRequired = true)]
        public string secondName => this[nameof(secondName)] as string;

        [ConfigurationProperty(nameof(id), IsRequired = true)]
        public int? id => this[nameof(id)] as int?;
    }
}