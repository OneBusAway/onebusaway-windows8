﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneBusAway.DataAccess.Test {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OneBusAway.DataAccess.Test.strings", typeof(strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;response&gt;
        ///              &lt;version&gt;1&lt;/version&gt;
        ///              &lt;code&gt;200&lt;/code&gt;
        ///              &lt;currentTime&gt;1348786676170&lt;/currentTime&gt;
        ///              &lt;text&gt;OK&lt;/text&gt;
        ///              &lt;data class=&quot;org.onebusaway.transit_data.model.StopsBean&quot;&gt;
        ///                &lt;stops&gt;
        ///                  &lt;stop&gt;
        ///                    &lt;id&gt;1_10914&lt;/id&gt;
        ///                    &lt;lat&gt;47.656426&lt;/lat&gt;
        ///                    &lt;lon&gt;-122.312164&lt;/lon&gt;
        ///                    &lt;direction&gt;S&lt;/direction&gt;
        ///                    &lt;name&gt;15TH AVE NE &amp;amp; NE CAM [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getStopsXml {
            get {
                return ResourceManager.GetString("getStopsXml", resourceCulture);
            }
        }
    }
}
