﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Excursions.Application.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExcursionResource_ru_RU {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExcursionResource_ru_RU() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Excursions.Application.Resources.ExcursionResource_ru_RU", typeof(ExcursionResource_ru_RU).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string Domain_BookingApproveWhenNotBookedError {
            get {
                return ResourceManager.GetString("Domain:BookingApproveWhenNotBookedError", resourceCulture);
            }
        }
        
        internal static string Domain_BookingRejectWhenNotBookedError {
            get {
                return ResourceManager.GetString("Domain:BookingRejectWhenNotBookedError", resourceCulture);
            }
        }
        
        internal static string Domain_ExcursionUpdateWhenNotDraftError {
            get {
                return ResourceManager.GetString("Domain:ExcursionUpdateWhenNotDraftError", resourceCulture);
            }
        }
        
        internal static string Domain_ExcursionBookWhenNotPublishedError {
            get {
                return ResourceManager.GetString("Domain:ExcursionBookWhenNotPublishedError", resourceCulture);
            }
        }
        
        internal static string Domain_ExcursionBookPlacesCountLimitError {
            get {
                return ResourceManager.GetString("Domain:ExcursionBookPlacesCountLimitError", resourceCulture);
            }
        }
        
        internal static string Validation_ExcursionDateTimeUtcNotDefaultError {
            get {
                return ResourceManager.GetString("Validation:ExcursionDateTimeUtcNotDefaultError", resourceCulture);
            }
        }
        
        internal static string Validation_ExcursionDescriptionMaximumLengthError {
            get {
                return ResourceManager.GetString("Validation:ExcursionDescriptionMaximumLengthError", resourceCulture);
            }
        }
        
        internal static string Validation_ExcursionNameMaximumLengthError {
            get {
                return ResourceManager.GetString("Validation:ExcursionNameMaximumLengthError", resourceCulture);
            }
        }
        
        internal static string Validation_ExcursionNameNotEmptyError {
            get {
                return ResourceManager.GetString("Validation:ExcursionNameNotEmptyError", resourceCulture);
            }
        }
        
        internal static string Validation_ExcursionPlacesCountGreaterThanError {
            get {
                return ResourceManager.GetString("Validation:ExcursionPlacesCountGreaterThanError", resourceCulture);
            }
        }
        
        internal static string Validation_ExcursionPricePerPlaceGreaterThanError {
            get {
                return ResourceManager.GetString("Validation:ExcursionPricePerPlaceGreaterThanError", resourceCulture);
            }
        }
        
        internal static string Domain_ExcursionBookWhenTouristAlreadyBookedError {
            get {
                return ResourceManager.GetString("Domain:ExcursionBookWhenTouristAlreadyBookedError", resourceCulture);
            }
        }
        
        internal static string Domain_ExcursionPublishWhenNotDraftError {
            get {
                return ResourceManager.GetString("Domain:ExcursionPublishWhenNotDraftError", resourceCulture);
            }
        }
    }
}
