﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YksMC.Data.Json {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("YksMC.Data.Json.Resources", typeof(Resources).GetTypeInfo().Assembly);
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
        ///   Looks up a localized string similar to [
        ///    {
        ///        &quot;Id&quot;: 0,
        ///        &quot;Name&quot;: &quot;Ocean&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 1,
        ///        &quot;Name&quot;: &quot;Plains&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 2,
        ///        &quot;Name&quot;: &quot;Desert&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 3,
        ///        &quot;Name&quot;: &quot;Extreme Hills&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 4,
        ///        &quot;Name&quot;: &quot;Forest&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 5,
        ///        &quot;Name&quot;: &quot;Taiga&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 6,
        ///        &quot;Name&quot;: &quot;Swampland&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 7,
        ///        &quot;Name&quot;: &quot;River&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 8,
        ///        &quot;Name&quot;: &quot;Hell&quot;
        ///    },
        ///    {
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Biomes {
            get {
                return ResourceManager.GetString("Biomes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///    {
        ///        &quot;Id&quot;: 0,
        ///        &quot;Name&quot;: &quot;air&quot;,
        ///        &quot;IsSolid&quot;: false,
        ///        &quot;IsDiggable&quot;: false,
        ///        &quot;Hardness&quot;: 0,
        ///        &quot;Tier&quot;: 0,
        ///        &quot;Material&quot;: &quot;Normal&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 1,
        ///        &quot;Name&quot;: &quot;stone&quot;,
        ///        &quot;IsSolid&quot;: true,
        ///        &quot;IsDiggable&quot;: true,
        ///        &quot;Hardness&quot;: 1.5,
        ///        &quot;Tier&quot;: 1,
        ///        &quot;Material&quot;: &quot;Rock&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 2,
        ///        &quot;Name&quot;: &quot;grass&quot;,
        ///        &quot;IsSolid&quot;: true,
        ///        &quot;IsDiggable&quot;: true,
        ///        &quot;Hardness&quot;: 0.6,
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BlockTypes {
            get {
                return ResourceManager.GetString("BlockTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///    {
        ///        &quot;Id&quot;: 1,
        ///        &quot;Name&quot;: &quot;item&quot;,
        ///        &quot;Type&quot;: &quot;mob&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 2,
        ///        &quot;Name&quot;: &quot;xp_orb&quot;,
        ///        &quot;Type&quot;: &quot;mob&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 3,
        ///        &quot;Name&quot;: &quot;area_effect_cloud&quot;,
        ///        &quot;Type&quot;: &quot;mob&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 4,
        ///        &quot;Name&quot;: &quot;elder_guardian&quot;,
        ///        &quot;Type&quot;: &quot;mob&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 5,
        ///        &quot;Name&quot;: &quot;wither_skeleton&quot;,
        ///        &quot;Type&quot;: &quot;mob&quot;
        ///    },
        ///    {
        ///        &quot;Id&quot;: 6,
        ///        &quot;Name&quot;: &quot;stray&quot;,
        ///        &quot;Type&quot;: &quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EntityTypes {
            get {
                return ResourceManager.GetString("EntityTypes", resourceCulture);
            }
        }
    }
}
