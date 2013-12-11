using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Shared.Services
{
    public class ServiceRepository
    {
        private static ServiceRepository instance = new ServiceRepository();
        private IFileService fileService;
        private INetworkService networkService;
        private ISettingsService settingsService;
        private IGeoLocationService geoLocationService;

        private ServiceRepository()
        {
        }
        
        public static IFileService FileService
        {
            get
            {
                return instance.fileService;
            }
            set
            {
                instance.fileService = value;
            }
        }

        public static INetworkService NetworkService
        {
            get
            {
                return instance.networkService;
            }
            set
            {
                instance.networkService = value;
            }
        }

        public static ISettingsService SettingsService
        {
            get
            {
                return instance.settingsService;
            }
            set
            {
                instance.settingsService = value;
            }
        }

        public static IGeoLocationService GeoLocationService
        {
            get
            {
                return instance.geoLocationService;
            }
            set
            {
                instance.geoLocationService = value;
            }
        }
    }
}
