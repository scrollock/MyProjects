using System.Configuration;

namespace SupportIndeed.Configuration
{
    public class ListEmploees : ConfigurationSection
    {
        [ConfigurationProperty(nameof(Emploees), IsRequired = false)]
        public EmploeesCollection Emploees
        {
            get { return this[nameof(Emploees)] as EmploeesCollection; }
        }
    }
}