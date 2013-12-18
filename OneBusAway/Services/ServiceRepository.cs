/* Copyright 2013 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Services
{
    public class ServiceRepository
    {
        private static ServiceRepository instance = new ServiceRepository();
        private IFileService fileService;
        private INetworkService networkService;
        private ISettingsService settingsService;
        private IGeoLocationService geoLocationService;
        private IPageControlService pageControlService;
        private IMessageBoxService messageBoxService;
        private ITileService tileService;

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

        public static IPageControlService PageControlService
        {
            get
            {
                return instance.pageControlService;
            }
            set
            {
                instance.pageControlService = value;
            }
        }

        public static IMessageBoxService MessageBoxService
        {
            get
            {
                return instance.messageBoxService;
            }
            set
            {
                instance.messageBoxService = value;
            }
        }

        public static ITileService TileService
        {
            get
            {
                return instance.tileService;
            }
            set
            {
                instance.tileService = value;
            }
        }
    }
}
