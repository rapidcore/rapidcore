using System.Collections.Generic;
using System.Linq;

namespace RapidCore.Globalization
{
    /// <summary>
    /// Country definitions as defined by ISO 3166-1
    /// https://en.wikipedia.org/wiki/ISO_3166-1
    /// </summary>
    public class Iso3166Countries
    {
        private readonly List<CountryIso3166> countries = new List<CountryIso3166>
        {
            new CountryIso3166 { CodeAlpha2 = "AF", CodeAlpha3 = "AFG", CodeNumeric = 004, NameEnglish = "Afghanistan" },
            new CountryIso3166 { CodeAlpha2 = "AX", CodeAlpha3 = "ALA", CodeNumeric = 248, NameEnglish = "Åland Islands" },
            new CountryIso3166 { CodeAlpha2 = "AL", CodeAlpha3 = "ALB", CodeNumeric = 008, NameEnglish = "Albania" },
            new CountryIso3166 { CodeAlpha2 = "DZ", CodeAlpha3 = "DZA", CodeNumeric = 012, NameEnglish = "Algeria" },
            new CountryIso3166 { CodeAlpha2 = "AS", CodeAlpha3 = "ASM", CodeNumeric = 016, NameEnglish = "American Samoa" },
            new CountryIso3166 { CodeAlpha2 = "AD", CodeAlpha3 = "AND", CodeNumeric = 020, NameEnglish = "Andorra" },
            new CountryIso3166 { CodeAlpha2 = "AO", CodeAlpha3 = "AGO", CodeNumeric = 024, NameEnglish = "Angola" },
            new CountryIso3166 { CodeAlpha2 = "AI", CodeAlpha3 = "AIA", CodeNumeric = 660, NameEnglish = "Anguilla" },
            new CountryIso3166 { CodeAlpha2 = "AQ", CodeAlpha3 = "ATA", CodeNumeric = 010, NameEnglish = "Antarctica" },
            new CountryIso3166 { CodeAlpha2 = "AG", CodeAlpha3 = "ATG", CodeNumeric = 028, NameEnglish = "Antigua and Barbuda" },
            new CountryIso3166 { CodeAlpha2 = "AR", CodeAlpha3 = "ARG", CodeNumeric = 032, NameEnglish = "Argentina" },
            new CountryIso3166 { CodeAlpha2 = "AM", CodeAlpha3 = "ARM", CodeNumeric = 051, NameEnglish = "Armenia" },
            new CountryIso3166 { CodeAlpha2 = "AW", CodeAlpha3 = "ABW", CodeNumeric = 533, NameEnglish = "Aruba" },
            new CountryIso3166 { CodeAlpha2 = "AU", CodeAlpha3 = "AUS", CodeNumeric = 036, NameEnglish = "Australia" },
            new CountryIso3166 { CodeAlpha2 = "AT", CodeAlpha3 = "AUT", CodeNumeric = 040, NameEnglish = "Austria" },
            new CountryIso3166 { CodeAlpha2 = "AZ", CodeAlpha3 = "AZE", CodeNumeric = 031, NameEnglish = "Azerbaijan" },
            new CountryIso3166 { CodeAlpha2 = "BS", CodeAlpha3 = "BHS", CodeNumeric = 044, NameEnglish = "Bahamas" },
            new CountryIso3166 { CodeAlpha2 = "BH", CodeAlpha3 = "BHR", CodeNumeric = 048, NameEnglish = "Bahrain" },
            new CountryIso3166 { CodeAlpha2 = "BD", CodeAlpha3 = "BGD", CodeNumeric = 050, NameEnglish = "Bangladesh" },
            new CountryIso3166 { CodeAlpha2 = "BB", CodeAlpha3 = "BRB", CodeNumeric = 052, NameEnglish = "Barbados" },
            new CountryIso3166 { CodeAlpha2 = "BY", CodeAlpha3 = "BLR", CodeNumeric = 112, NameEnglish = "Belarus" },
            new CountryIso3166 { CodeAlpha2 = "BE", CodeAlpha3 = "BEL", CodeNumeric = 056, NameEnglish = "Belgium" },
            new CountryIso3166 { CodeAlpha2 = "BZ", CodeAlpha3 = "BLZ", CodeNumeric = 084, NameEnglish = "Belize" },
            new CountryIso3166 { CodeAlpha2 = "BJ", CodeAlpha3 = "BEN", CodeNumeric = 204, NameEnglish = "Benin" },
            new CountryIso3166 { CodeAlpha2 = "BM", CodeAlpha3 = "BMU", CodeNumeric = 060, NameEnglish = "Bermuda" },
            new CountryIso3166 { CodeAlpha2 = "BT", CodeAlpha3 = "BTN", CodeNumeric = 064, NameEnglish = "Bhutan" },
            new CountryIso3166 { CodeAlpha2 = "BO", CodeAlpha3 = "BOL", CodeNumeric = 068, NameEnglish = "Bolivia (Plurinational State of)" },
            new CountryIso3166 { CodeAlpha2 = "BQ", CodeAlpha3 = "BES", CodeNumeric = 535, NameEnglish = "Bonaire, Sint Eustatius and Saba" },
            new CountryIso3166 { CodeAlpha2 = "BA", CodeAlpha3 = "BIH", CodeNumeric = 070, NameEnglish = "Bosnia and Herzegovina" },
            new CountryIso3166 { CodeAlpha2 = "BW", CodeAlpha3 = "BWA", CodeNumeric = 072, NameEnglish = "Botswana" },
            new CountryIso3166 { CodeAlpha2 = "BV", CodeAlpha3 = "BVT", CodeNumeric = 074, NameEnglish = "Bouvet Island" },
            new CountryIso3166 { CodeAlpha2 = "BR", CodeAlpha3 = "BRA", CodeNumeric = 076, NameEnglish = "Brazil" },
            new CountryIso3166 { CodeAlpha2 = "IO", CodeAlpha3 = "IOT", CodeNumeric = 086, NameEnglish = "British Indian Ocean Territory" },
            new CountryIso3166 { CodeAlpha2 = "BN", CodeAlpha3 = "BRN", CodeNumeric = 096, NameEnglish = "Brunei Darussalam" },
            new CountryIso3166 { CodeAlpha2 = "BG", CodeAlpha3 = "BGR", CodeNumeric = 100, NameEnglish = "Bulgaria" },
            new CountryIso3166 { CodeAlpha2 = "BF", CodeAlpha3 = "BFA", CodeNumeric = 854, NameEnglish = "Burkina Faso" },
            new CountryIso3166 { CodeAlpha2 = "BI", CodeAlpha3 = "BDI", CodeNumeric = 108, NameEnglish = "Burundi" },
            new CountryIso3166 { CodeAlpha2 = "CV", CodeAlpha3 = "CPV", CodeNumeric = 132, NameEnglish = "Cabo Verde" },
            new CountryIso3166 { CodeAlpha2 = "KH", CodeAlpha3 = "KHM", CodeNumeric = 116, NameEnglish = "Cambodia" },
            new CountryIso3166 { CodeAlpha2 = "CM", CodeAlpha3 = "CMR", CodeNumeric = 120, NameEnglish = "Cameroon" },
            new CountryIso3166 { CodeAlpha2 = "CA", CodeAlpha3 = "CAN", CodeNumeric = 124, NameEnglish = "Canada" },
            new CountryIso3166 { CodeAlpha2 = "KY", CodeAlpha3 = "CYM", CodeNumeric = 136, NameEnglish = "Cayman Islands" },
            new CountryIso3166 { CodeAlpha2 = "CF", CodeAlpha3 = "CAF", CodeNumeric = 140, NameEnglish = "Central African Republic" },
            new CountryIso3166 { CodeAlpha2 = "TD", CodeAlpha3 = "TCD", CodeNumeric = 148, NameEnglish = "Chad" },
            new CountryIso3166 { CodeAlpha2 = "CL", CodeAlpha3 = "CHL", CodeNumeric = 152, NameEnglish = "Chile" },
            new CountryIso3166 { CodeAlpha2 = "CN", CodeAlpha3 = "CHN", CodeNumeric = 156, NameEnglish = "China" },
            new CountryIso3166 { CodeAlpha2 = "CX", CodeAlpha3 = "CXR", CodeNumeric = 162, NameEnglish = "Christmas Island" },
            new CountryIso3166 { CodeAlpha2 = "CC", CodeAlpha3 = "CCK", CodeNumeric = 166, NameEnglish = "Cocos (Keeling) Islands" },
            new CountryIso3166 { CodeAlpha2 = "CO", CodeAlpha3 = "COL", CodeNumeric = 170, NameEnglish = "Colombia" },
            new CountryIso3166 { CodeAlpha2 = "KM", CodeAlpha3 = "COM", CodeNumeric = 174, NameEnglish = "Comoros" },
            new CountryIso3166 { CodeAlpha2 = "CG", CodeAlpha3 = "COG", CodeNumeric = 178, NameEnglish = "Congo" },
            new CountryIso3166 { CodeAlpha2 = "CD", CodeAlpha3 = "COD", CodeNumeric = 180, NameEnglish = "Congo (Democratic Republic of the)" },
            new CountryIso3166 { CodeAlpha2 = "CK", CodeAlpha3 = "COK", CodeNumeric = 184, NameEnglish = "Cook Islands" },
            new CountryIso3166 { CodeAlpha2 = "CR", CodeAlpha3 = "CRI", CodeNumeric = 188, NameEnglish = "Costa Rica" },
            new CountryIso3166 { CodeAlpha2 = "CI", CodeAlpha3 = "CIV", CodeNumeric = 384, NameEnglish = "Côte d'Ivoire" },
            new CountryIso3166 { CodeAlpha2 = "HR", CodeAlpha3 = "HRV", CodeNumeric = 191, NameEnglish = "Croatia" },
            new CountryIso3166 { CodeAlpha2 = "CU", CodeAlpha3 = "CUB", CodeNumeric = 192, NameEnglish = "Cuba" },
            new CountryIso3166 { CodeAlpha2 = "CW", CodeAlpha3 = "CUW", CodeNumeric = 531, NameEnglish = "Curaçao" },
            new CountryIso3166 { CodeAlpha2 = "CY", CodeAlpha3 = "CYP", CodeNumeric = 196, NameEnglish = "Cyprus" },
            new CountryIso3166 { CodeAlpha2 = "CZ", CodeAlpha3 = "CZE", CodeNumeric = 203, NameEnglish = "Czechia" },
            new CountryIso3166 { CodeAlpha2 = "DK", CodeAlpha3 = "DNK", CodeNumeric = 208, NameEnglish = "Denmark" },
            new CountryIso3166 { CodeAlpha2 = "DJ", CodeAlpha3 = "DJI", CodeNumeric = 262, NameEnglish = "Djibouti" },
            new CountryIso3166 { CodeAlpha2 = "DM", CodeAlpha3 = "DMA", CodeNumeric = 212, NameEnglish = "Dominica" },
            new CountryIso3166 { CodeAlpha2 = "DO", CodeAlpha3 = "DOM", CodeNumeric = 214, NameEnglish = "Dominican Republic" },
            new CountryIso3166 { CodeAlpha2 = "EC", CodeAlpha3 = "ECU", CodeNumeric = 218, NameEnglish = "Ecuador" },
            new CountryIso3166 { CodeAlpha2 = "EG", CodeAlpha3 = "EGY", CodeNumeric = 818, NameEnglish = "Egypt" },
            new CountryIso3166 { CodeAlpha2 = "SV", CodeAlpha3 = "SLV", CodeNumeric = 222, NameEnglish = "El Salvador" },
            new CountryIso3166 { CodeAlpha2 = "GQ", CodeAlpha3 = "GNQ", CodeNumeric = 226, NameEnglish = "Equatorial Guinea" },
            new CountryIso3166 { CodeAlpha2 = "ER", CodeAlpha3 = "ERI", CodeNumeric = 232, NameEnglish = "Eritrea" },
            new CountryIso3166 { CodeAlpha2 = "EE", CodeAlpha3 = "EST", CodeNumeric = 233, NameEnglish = "Estonia" },
            new CountryIso3166 { CodeAlpha2 = "ET", CodeAlpha3 = "ETH", CodeNumeric = 231, NameEnglish = "Ethiopia" },
            new CountryIso3166 { CodeAlpha2 = "FK", CodeAlpha3 = "FLK", CodeNumeric = 238, NameEnglish = "Falkland Islands (Malvinas)" },
            new CountryIso3166 { CodeAlpha2 = "FO", CodeAlpha3 = "FRO", CodeNumeric = 234, NameEnglish = "Faroe Islands" },
            new CountryIso3166 { CodeAlpha2 = "FJ", CodeAlpha3 = "FJI", CodeNumeric = 242, NameEnglish = "Fiji" },
            new CountryIso3166 { CodeAlpha2 = "FI", CodeAlpha3 = "FIN", CodeNumeric = 246, NameEnglish = "Finland" },
            new CountryIso3166 { CodeAlpha2 = "FR", CodeAlpha3 = "FRA", CodeNumeric = 250, NameEnglish = "France" },
            new CountryIso3166 { CodeAlpha2 = "GF", CodeAlpha3 = "GUF", CodeNumeric = 254, NameEnglish = "French Guiana" },
            new CountryIso3166 { CodeAlpha2 = "PF", CodeAlpha3 = "PYF", CodeNumeric = 258, NameEnglish = "French Polynesia" },
            new CountryIso3166 { CodeAlpha2 = "TF", CodeAlpha3 = "ATF", CodeNumeric = 260, NameEnglish = "French Southern Territories" },
            new CountryIso3166 { CodeAlpha2 = "GA", CodeAlpha3 = "GAB", CodeNumeric = 266, NameEnglish = "Gabon" },
            new CountryIso3166 { CodeAlpha2 = "GM", CodeAlpha3 = "GMB", CodeNumeric = 270, NameEnglish = "Gambia" },
            new CountryIso3166 { CodeAlpha2 = "GE", CodeAlpha3 = "GEO", CodeNumeric = 268, NameEnglish = "Georgia" },
            new CountryIso3166 { CodeAlpha2 = "DE", CodeAlpha3 = "DEU", CodeNumeric = 276, NameEnglish = "Germany" },
            new CountryIso3166 { CodeAlpha2 = "GH", CodeAlpha3 = "GHA", CodeNumeric = 288, NameEnglish = "Ghana" },
            new CountryIso3166 { CodeAlpha2 = "GI", CodeAlpha3 = "GIB", CodeNumeric = 292, NameEnglish = "Gibraltar" },
            new CountryIso3166 { CodeAlpha2 = "GR", CodeAlpha3 = "GRC", CodeNumeric = 300, NameEnglish = "Greece" },
            new CountryIso3166 { CodeAlpha2 = "GL", CodeAlpha3 = "GRL", CodeNumeric = 304, NameEnglish = "Greenland" },
            new CountryIso3166 { CodeAlpha2 = "GD", CodeAlpha3 = "GRD", CodeNumeric = 308, NameEnglish = "Grenada" },
            new CountryIso3166 { CodeAlpha2 = "GP", CodeAlpha3 = "GLP", CodeNumeric = 312, NameEnglish = "Guadeloupe" },
            new CountryIso3166 { CodeAlpha2 = "GU", CodeAlpha3 = "GUM", CodeNumeric = 316, NameEnglish = "Guam" },
            new CountryIso3166 { CodeAlpha2 = "GT", CodeAlpha3 = "GTM", CodeNumeric = 320, NameEnglish = "Guatemala" },
            new CountryIso3166 { CodeAlpha2 = "GG", CodeAlpha3 = "GGY", CodeNumeric = 831, NameEnglish = "Guernsey" },
            new CountryIso3166 { CodeAlpha2 = "GN", CodeAlpha3 = "GIN", CodeNumeric = 324, NameEnglish = "Guinea" },
            new CountryIso3166 { CodeAlpha2 = "GW", CodeAlpha3 = "GNB", CodeNumeric = 624, NameEnglish = "Guinea-Bissau" },
            new CountryIso3166 { CodeAlpha2 = "GY", CodeAlpha3 = "GUY", CodeNumeric = 328, NameEnglish = "Guyana" },
            new CountryIso3166 { CodeAlpha2 = "HT", CodeAlpha3 = "HTI", CodeNumeric = 332, NameEnglish = "Haiti" },
            new CountryIso3166 { CodeAlpha2 = "HM", CodeAlpha3 = "HMD", CodeNumeric = 334, NameEnglish = "Heard Island and McDonald Islands" },
            new CountryIso3166 { CodeAlpha2 = "VA", CodeAlpha3 = "VAT", CodeNumeric = 336, NameEnglish = "Holy See" },
            new CountryIso3166 { CodeAlpha2 = "HN", CodeAlpha3 = "HND", CodeNumeric = 340, NameEnglish = "Honduras" },
            new CountryIso3166 { CodeAlpha2 = "HK", CodeAlpha3 = "HKG", CodeNumeric = 344, NameEnglish = "Hong Kong" },
            new CountryIso3166 { CodeAlpha2 = "HU", CodeAlpha3 = "HUN", CodeNumeric = 348, NameEnglish = "Hungary" },
            new CountryIso3166 { CodeAlpha2 = "IS", CodeAlpha3 = "ISL", CodeNumeric = 352, NameEnglish = "Iceland" },
            new CountryIso3166 { CodeAlpha2 = "IN", CodeAlpha3 = "IND", CodeNumeric = 356, NameEnglish = "India" },
            new CountryIso3166 { CodeAlpha2 = "ID", CodeAlpha3 = "IDN", CodeNumeric = 360, NameEnglish = "Indonesia" },
            new CountryIso3166 { CodeAlpha2 = "IR", CodeAlpha3 = "IRN", CodeNumeric = 364, NameEnglish = "Iran (Islamic Republic of)" },
            new CountryIso3166 { CodeAlpha2 = "IQ", CodeAlpha3 = "IRQ", CodeNumeric = 368, NameEnglish = "Iraq" },
            new CountryIso3166 { CodeAlpha2 = "IE", CodeAlpha3 = "IRL", CodeNumeric = 372, NameEnglish = "Ireland" },
            new CountryIso3166 { CodeAlpha2 = "IM", CodeAlpha3 = "IMN", CodeNumeric = 833, NameEnglish = "Isle of Man" },
            new CountryIso3166 { CodeAlpha2 = "IL", CodeAlpha3 = "ISR", CodeNumeric = 376, NameEnglish = "Israel" },
            new CountryIso3166 { CodeAlpha2 = "IT", CodeAlpha3 = "ITA", CodeNumeric = 380, NameEnglish = "Italy" },
            new CountryIso3166 { CodeAlpha2 = "JM", CodeAlpha3 = "JAM", CodeNumeric = 388, NameEnglish = "Jamaica" },
            new CountryIso3166 { CodeAlpha2 = "JP", CodeAlpha3 = "JPN", CodeNumeric = 392, NameEnglish = "Japan" },
            new CountryIso3166 { CodeAlpha2 = "JE", CodeAlpha3 = "JEY", CodeNumeric = 832, NameEnglish = "Jersey" },
            new CountryIso3166 { CodeAlpha2 = "JO", CodeAlpha3 = "JOR", CodeNumeric = 400, NameEnglish = "Jordan" },
            new CountryIso3166 { CodeAlpha2 = "KZ", CodeAlpha3 = "KAZ", CodeNumeric = 398, NameEnglish = "Kazakhstan" },
            new CountryIso3166 { CodeAlpha2 = "KE", CodeAlpha3 = "KEN", CodeNumeric = 404, NameEnglish = "Kenya" },
            new CountryIso3166 { CodeAlpha2 = "KI", CodeAlpha3 = "KIR", CodeNumeric = 296, NameEnglish = "Kiribati" },
            new CountryIso3166 { CodeAlpha2 = "KP", CodeAlpha3 = "PRK", CodeNumeric = 408, NameEnglish = "Korea (Democratic People's Republic of)" },
            new CountryIso3166 { CodeAlpha2 = "KR", CodeAlpha3 = "KOR", CodeNumeric = 410, NameEnglish = "Korea (Republic of)" },
            new CountryIso3166 { CodeAlpha2 = "KW", CodeAlpha3 = "KWT", CodeNumeric = 414, NameEnglish = "Kuwait" },
            new CountryIso3166 { CodeAlpha2 = "KG", CodeAlpha3 = "KGZ", CodeNumeric = 417, NameEnglish = "Kyrgyzstan" },
            new CountryIso3166 { CodeAlpha2 = "LA", CodeAlpha3 = "LAO", CodeNumeric = 418, NameEnglish = "Lao People's Democratic Republic" },
            new CountryIso3166 { CodeAlpha2 = "LV", CodeAlpha3 = "LVA", CodeNumeric = 428, NameEnglish = "Latvia" },
            new CountryIso3166 { CodeAlpha2 = "LB", CodeAlpha3 = "LBN", CodeNumeric = 422, NameEnglish = "Lebanon" },
            new CountryIso3166 { CodeAlpha2 = "LS", CodeAlpha3 = "LSO", CodeNumeric = 426, NameEnglish = "Lesotho" },
            new CountryIso3166 { CodeAlpha2 = "LR", CodeAlpha3 = "LBR", CodeNumeric = 430, NameEnglish = "Liberia" },
            new CountryIso3166 { CodeAlpha2 = "LY", CodeAlpha3 = "LBY", CodeNumeric = 434, NameEnglish = "Libya" },
            new CountryIso3166 { CodeAlpha2 = "LI", CodeAlpha3 = "LIE", CodeNumeric = 438, NameEnglish = "Liechtenstein" },
            new CountryIso3166 { CodeAlpha2 = "LT", CodeAlpha3 = "LTU", CodeNumeric = 440, NameEnglish = "Lithuania" },
            new CountryIso3166 { CodeAlpha2 = "LU", CodeAlpha3 = "LUX", CodeNumeric = 442, NameEnglish = "Luxembourg" },
            new CountryIso3166 { CodeAlpha2 = "MO", CodeAlpha3 = "MAC", CodeNumeric = 446, NameEnglish = "Macao" },
            new CountryIso3166 { CodeAlpha2 = "MK", CodeAlpha3 = "MKD", CodeNumeric = 807, NameEnglish = "Macedonia (the former Yugoslav Republic of)" },
            new CountryIso3166 { CodeAlpha2 = "MG", CodeAlpha3 = "MDG", CodeNumeric = 450, NameEnglish = "Madagascar" },
            new CountryIso3166 { CodeAlpha2 = "MW", CodeAlpha3 = "MWI", CodeNumeric = 454, NameEnglish = "Malawi" },
            new CountryIso3166 { CodeAlpha2 = "MY", CodeAlpha3 = "MYS", CodeNumeric = 458, NameEnglish = "Malaysia" },
            new CountryIso3166 { CodeAlpha2 = "MV", CodeAlpha3 = "MDV", CodeNumeric = 462, NameEnglish = "Maldives" },
            new CountryIso3166 { CodeAlpha2 = "ML", CodeAlpha3 = "MLI", CodeNumeric = 466, NameEnglish = "Mali" },
            new CountryIso3166 { CodeAlpha2 = "MT", CodeAlpha3 = "MLT", CodeNumeric = 470, NameEnglish = "Malta" },
            new CountryIso3166 { CodeAlpha2 = "MH", CodeAlpha3 = "MHL", CodeNumeric = 584, NameEnglish = "Marshall Islands" },
            new CountryIso3166 { CodeAlpha2 = "MQ", CodeAlpha3 = "MTQ", CodeNumeric = 474, NameEnglish = "Martinique" },
            new CountryIso3166 { CodeAlpha2 = "MR", CodeAlpha3 = "MRT", CodeNumeric = 478, NameEnglish = "Mauritania" },
            new CountryIso3166 { CodeAlpha2 = "MU", CodeAlpha3 = "MUS", CodeNumeric = 480, NameEnglish = "Mauritius" },
            new CountryIso3166 { CodeAlpha2 = "YT", CodeAlpha3 = "MYT", CodeNumeric = 175, NameEnglish = "Mayotte" },
            new CountryIso3166 { CodeAlpha2 = "MX", CodeAlpha3 = "MEX", CodeNumeric = 484, NameEnglish = "Mexico" },
            new CountryIso3166 { CodeAlpha2 = "FM", CodeAlpha3 = "FSM", CodeNumeric = 583, NameEnglish = "Micronesia (Federated States of)" },
            new CountryIso3166 { CodeAlpha2 = "MD", CodeAlpha3 = "MDA", CodeNumeric = 498, NameEnglish = "Moldova (Republic of)" },
            new CountryIso3166 { CodeAlpha2 = "MC", CodeAlpha3 = "MCO", CodeNumeric = 492, NameEnglish = "Monaco" },
            new CountryIso3166 { CodeAlpha2 = "MN", CodeAlpha3 = "MNG", CodeNumeric = 496, NameEnglish = "Mongolia" },
            new CountryIso3166 { CodeAlpha2 = "ME", CodeAlpha3 = "MNE", CodeNumeric = 499, NameEnglish = "Montenegro" },
            new CountryIso3166 { CodeAlpha2 = "MS", CodeAlpha3 = "MSR", CodeNumeric = 500, NameEnglish = "Montserrat" },
            new CountryIso3166 { CodeAlpha2 = "MA", CodeAlpha3 = "MAR", CodeNumeric = 504, NameEnglish = "Morocco" },
            new CountryIso3166 { CodeAlpha2 = "MZ", CodeAlpha3 = "MOZ", CodeNumeric = 508, NameEnglish = "Mozambique" },
            new CountryIso3166 { CodeAlpha2 = "MM", CodeAlpha3 = "MMR", CodeNumeric = 104, NameEnglish = "Myanmar" },
            new CountryIso3166 { CodeAlpha2 = "NA", CodeAlpha3 = "NAM", CodeNumeric = 516, NameEnglish = "Namibia" },
            new CountryIso3166 { CodeAlpha2 = "NR", CodeAlpha3 = "NRU", CodeNumeric = 520, NameEnglish = "Nauru" },
            new CountryIso3166 { CodeAlpha2 = "NP", CodeAlpha3 = "NPL", CodeNumeric = 524, NameEnglish = "Nepal" },
            new CountryIso3166 { CodeAlpha2 = "NL", CodeAlpha3 = "NLD", CodeNumeric = 528, NameEnglish = "Netherlands" },
            new CountryIso3166 { CodeAlpha2 = "NC", CodeAlpha3 = "NCL", CodeNumeric = 540, NameEnglish = "New Caledonia" },
            new CountryIso3166 { CodeAlpha2 = "NZ", CodeAlpha3 = "NZL", CodeNumeric = 554, NameEnglish = "New Zealand" },
            new CountryIso3166 { CodeAlpha2 = "NI", CodeAlpha3 = "NIC", CodeNumeric = 558, NameEnglish = "Nicaragua" },
            new CountryIso3166 { CodeAlpha2 = "NE", CodeAlpha3 = "NER", CodeNumeric = 562, NameEnglish = "Niger" },
            new CountryIso3166 { CodeAlpha2 = "NG", CodeAlpha3 = "NGA", CodeNumeric = 566, NameEnglish = "Nigeria" },
            new CountryIso3166 { CodeAlpha2 = "NU", CodeAlpha3 = "NIU", CodeNumeric = 570, NameEnglish = "Niue" },
            new CountryIso3166 { CodeAlpha2 = "NF", CodeAlpha3 = "NFK", CodeNumeric = 574, NameEnglish = "Norfolk Island" },
            new CountryIso3166 { CodeAlpha2 = "MP", CodeAlpha3 = "MNP", CodeNumeric = 580, NameEnglish = "Northern Mariana Islands" },
            new CountryIso3166 { CodeAlpha2 = "NO", CodeAlpha3 = "NOR", CodeNumeric = 578, NameEnglish = "Norway" },
            new CountryIso3166 { CodeAlpha2 = "OM", CodeAlpha3 = "OMN", CodeNumeric = 512, NameEnglish = "Oman" },
            new CountryIso3166 { CodeAlpha2 = "PK", CodeAlpha3 = "PAK", CodeNumeric = 586, NameEnglish = "Pakistan" },
            new CountryIso3166 { CodeAlpha2 = "PW", CodeAlpha3 = "PLW", CodeNumeric = 585, NameEnglish = "Palau" },
            new CountryIso3166 { CodeAlpha2 = "PS", CodeAlpha3 = "PSE", CodeNumeric = 275, NameEnglish = "Palestine, State of" },
            new CountryIso3166 { CodeAlpha2 = "PA", CodeAlpha3 = "PAN", CodeNumeric = 591, NameEnglish = "Panama" },
            new CountryIso3166 { CodeAlpha2 = "PG", CodeAlpha3 = "PNG", CodeNumeric = 598, NameEnglish = "Papua New Guinea" },
            new CountryIso3166 { CodeAlpha2 = "PY", CodeAlpha3 = "PRY", CodeNumeric = 600, NameEnglish = "Paraguay" },
            new CountryIso3166 { CodeAlpha2 = "PE", CodeAlpha3 = "PER", CodeNumeric = 604, NameEnglish = "Peru" },
            new CountryIso3166 { CodeAlpha2 = "PH", CodeAlpha3 = "PHL", CodeNumeric = 608, NameEnglish = "Philippines" },
            new CountryIso3166 { CodeAlpha2 = "PN", CodeAlpha3 = "PCN", CodeNumeric = 612, NameEnglish = "Pitcairn" },
            new CountryIso3166 { CodeAlpha2 = "PL", CodeAlpha3 = "POL", CodeNumeric = 616, NameEnglish = "Poland" },
            new CountryIso3166 { CodeAlpha2 = "PT", CodeAlpha3 = "PRT", CodeNumeric = 620, NameEnglish = "Portugal" },
            new CountryIso3166 { CodeAlpha2 = "PR", CodeAlpha3 = "PRI", CodeNumeric = 630, NameEnglish = "Puerto Rico" },
            new CountryIso3166 { CodeAlpha2 = "QA", CodeAlpha3 = "QAT", CodeNumeric = 634, NameEnglish = "Qatar" },
            new CountryIso3166 { CodeAlpha2 = "RE", CodeAlpha3 = "REU", CodeNumeric = 638, NameEnglish = "Réunion" },
            new CountryIso3166 { CodeAlpha2 = "RO", CodeAlpha3 = "ROU", CodeNumeric = 642, NameEnglish = "Romania" },
            new CountryIso3166 { CodeAlpha2 = "RU", CodeAlpha3 = "RUS", CodeNumeric = 643, NameEnglish = "Russian Federation" },
            new CountryIso3166 { CodeAlpha2 = "RW", CodeAlpha3 = "RWA", CodeNumeric = 646, NameEnglish = "Rwanda" },
            new CountryIso3166 { CodeAlpha2 = "BL", CodeAlpha3 = "BLM", CodeNumeric = 652, NameEnglish = "Saint Barthélemy" },
            new CountryIso3166 { CodeAlpha2 = "SH", CodeAlpha3 = "SHN", CodeNumeric = 654, NameEnglish = "Saint Helena, Ascension and Tristan da Cunha" },
            new CountryIso3166 { CodeAlpha2 = "KN", CodeAlpha3 = "KNA", CodeNumeric = 659, NameEnglish = "Saint Kitts and Nevis" },
            new CountryIso3166 { CodeAlpha2 = "LC", CodeAlpha3 = "LCA", CodeNumeric = 662, NameEnglish = "Saint Lucia" },
            new CountryIso3166 { CodeAlpha2 = "MF", CodeAlpha3 = "MAF", CodeNumeric = 663, NameEnglish = "Saint Martin (French part)" },
            new CountryIso3166 { CodeAlpha2 = "PM", CodeAlpha3 = "SPM", CodeNumeric = 666, NameEnglish = "Saint Pierre and Miquelon" },
            new CountryIso3166 { CodeAlpha2 = "VC", CodeAlpha3 = "VCT", CodeNumeric = 670, NameEnglish = "Saint Vincent and the Grenadines" },
            new CountryIso3166 { CodeAlpha2 = "WS", CodeAlpha3 = "WSM", CodeNumeric = 882, NameEnglish = "Samoa" },
            new CountryIso3166 { CodeAlpha2 = "SM", CodeAlpha3 = "SMR", CodeNumeric = 674, NameEnglish = "San Marino" },
            new CountryIso3166 { CodeAlpha2 = "ST", CodeAlpha3 = "STP", CodeNumeric = 678, NameEnglish = "Sao Tome and Principe" },
            new CountryIso3166 { CodeAlpha2 = "SA", CodeAlpha3 = "SAU", CodeNumeric = 682, NameEnglish = "Saudi Arabia" },
            new CountryIso3166 { CodeAlpha2 = "SN", CodeAlpha3 = "SEN", CodeNumeric = 686, NameEnglish = "Senegal" },
            new CountryIso3166 { CodeAlpha2 = "RS", CodeAlpha3 = "SRB", CodeNumeric = 688, NameEnglish = "Serbia" },
            new CountryIso3166 { CodeAlpha2 = "SC", CodeAlpha3 = "SYC", CodeNumeric = 690, NameEnglish = "Seychelles" },
            new CountryIso3166 { CodeAlpha2 = "SL", CodeAlpha3 = "SLE", CodeNumeric = 694, NameEnglish = "Sierra Leone" },
            new CountryIso3166 { CodeAlpha2 = "SG", CodeAlpha3 = "SGP", CodeNumeric = 702, NameEnglish = "Singapore" },
            new CountryIso3166 { CodeAlpha2 = "SX", CodeAlpha3 = "SXM", CodeNumeric = 534, NameEnglish = "Sint Maarten (Dutch part)" },
            new CountryIso3166 { CodeAlpha2 = "SK", CodeAlpha3 = "SVK", CodeNumeric = 703, NameEnglish = "Slovakia" },
            new CountryIso3166 { CodeAlpha2 = "SI", CodeAlpha3 = "SVN", CodeNumeric = 705, NameEnglish = "Slovenia" },
            new CountryIso3166 { CodeAlpha2 = "SB", CodeAlpha3 = "SLB", CodeNumeric = 090, NameEnglish = "Solomon Islands" },
            new CountryIso3166 { CodeAlpha2 = "SO", CodeAlpha3 = "SOM", CodeNumeric = 706, NameEnglish = "Somalia" },
            new CountryIso3166 { CodeAlpha2 = "ZA", CodeAlpha3 = "ZAF", CodeNumeric = 710, NameEnglish = "South Africa" },
            new CountryIso3166 { CodeAlpha2 = "GS", CodeAlpha3 = "SGS", CodeNumeric = 239, NameEnglish = "South Georgia and the South Sandwich Islands" },
            new CountryIso3166 { CodeAlpha2 = "SS", CodeAlpha3 = "SSD", CodeNumeric = 728, NameEnglish = "South Sudan" },
            new CountryIso3166 { CodeAlpha2 = "ES", CodeAlpha3 = "ESP", CodeNumeric = 724, NameEnglish = "Spain" },
            new CountryIso3166 { CodeAlpha2 = "LK", CodeAlpha3 = "LKA", CodeNumeric = 144, NameEnglish = "Sri Lanka" },
            new CountryIso3166 { CodeAlpha2 = "SD", CodeAlpha3 = "SDN", CodeNumeric = 729, NameEnglish = "Sudan" },
            new CountryIso3166 { CodeAlpha2 = "SR", CodeAlpha3 = "SUR", CodeNumeric = 740, NameEnglish = "Suriname" },
            new CountryIso3166 { CodeAlpha2 = "SJ", CodeAlpha3 = "SJM", CodeNumeric = 744, NameEnglish = "Svalbard and Jan Mayen" },
            new CountryIso3166 { CodeAlpha2 = "SZ", CodeAlpha3 = "SWZ", CodeNumeric = 748, NameEnglish = "Swaziland" },
            new CountryIso3166 { CodeAlpha2 = "SE", CodeAlpha3 = "SWE", CodeNumeric = 752, NameEnglish = "Sweden" },
            new CountryIso3166 { CodeAlpha2 = "CH", CodeAlpha3 = "CHE", CodeNumeric = 756, NameEnglish = "Switzerland" },
            new CountryIso3166 { CodeAlpha2 = "SY", CodeAlpha3 = "SYR", CodeNumeric = 760, NameEnglish = "Syrian Arab Republic" },
            new CountryIso3166 { CodeAlpha2 = "TW", CodeAlpha3 = "TWN", CodeNumeric = 158, NameEnglish = "Taiwan, Province of China[a]" },
            new CountryIso3166 { CodeAlpha2 = "TJ", CodeAlpha3 = "TJK", CodeNumeric = 762, NameEnglish = "Tajikistan" },
            new CountryIso3166 { CodeAlpha2 = "TZ", CodeAlpha3 = "TZA", CodeNumeric = 834, NameEnglish = "Tanzania, United Republic of" },
            new CountryIso3166 { CodeAlpha2 = "TH", CodeAlpha3 = "THA", CodeNumeric = 764, NameEnglish = "Thailand" },
            new CountryIso3166 { CodeAlpha2 = "TL", CodeAlpha3 = "TLS", CodeNumeric = 626, NameEnglish = "Timor-Leste" },
            new CountryIso3166 { CodeAlpha2 = "TG", CodeAlpha3 = "TGO", CodeNumeric = 768, NameEnglish = "Togo" },
            new CountryIso3166 { CodeAlpha2 = "TK", CodeAlpha3 = "TKL", CodeNumeric = 772, NameEnglish = "Tokelau" },
            new CountryIso3166 { CodeAlpha2 = "TO", CodeAlpha3 = "TON", CodeNumeric = 776, NameEnglish = "Tonga" },
            new CountryIso3166 { CodeAlpha2 = "TT", CodeAlpha3 = "TTO", CodeNumeric = 780, NameEnglish = "Trinidad and Tobago" },
            new CountryIso3166 { CodeAlpha2 = "TN", CodeAlpha3 = "TUN", CodeNumeric = 788, NameEnglish = "Tunisia" },
            new CountryIso3166 { CodeAlpha2 = "TR", CodeAlpha3 = "TUR", CodeNumeric = 792, NameEnglish = "Turkey" },
            new CountryIso3166 { CodeAlpha2 = "TM", CodeAlpha3 = "TKM", CodeNumeric = 795, NameEnglish = "Turkmenistan" },
            new CountryIso3166 { CodeAlpha2 = "TC", CodeAlpha3 = "TCA", CodeNumeric = 796, NameEnglish = "Turks and Caicos Islands" },
            new CountryIso3166 { CodeAlpha2 = "TV", CodeAlpha3 = "TUV", CodeNumeric = 798, NameEnglish = "Tuvalu" },
            new CountryIso3166 { CodeAlpha2 = "UG", CodeAlpha3 = "UGA", CodeNumeric = 800, NameEnglish = "Uganda" },
            new CountryIso3166 { CodeAlpha2 = "UA", CodeAlpha3 = "UKR", CodeNumeric = 804, NameEnglish = "Ukraine" },
            new CountryIso3166 { CodeAlpha2 = "AE", CodeAlpha3 = "ARE", CodeNumeric = 784, NameEnglish = "United Arab Emirates" },
            new CountryIso3166 { CodeAlpha2 = "GB", CodeAlpha3 = "GBR", CodeNumeric = 826, NameEnglish = "United Kingdom of Great Britain and Northern Ireland" },
            new CountryIso3166 { CodeAlpha2 = "US", CodeAlpha3 = "USA", CodeNumeric = 840, NameEnglish = "United States of America" },
            new CountryIso3166 { CodeAlpha2 = "UM", CodeAlpha3 = "UMI", CodeNumeric = 581, NameEnglish = "United States Minor Outlying Islands" },
            new CountryIso3166 { CodeAlpha2 = "UY", CodeAlpha3 = "URY", CodeNumeric = 858, NameEnglish = "Uruguay" },
            new CountryIso3166 { CodeAlpha2 = "UZ", CodeAlpha3 = "UZB", CodeNumeric = 860, NameEnglish = "Uzbekistan" },
            new CountryIso3166 { CodeAlpha2 = "VU", CodeAlpha3 = "VUT", CodeNumeric = 548, NameEnglish = "Vanuatu" },
            new CountryIso3166 { CodeAlpha2 = "VE", CodeAlpha3 = "VEN", CodeNumeric = 862, NameEnglish = "Venezuela (Bolivarian Republic of)" },
            new CountryIso3166 { CodeAlpha2 = "VN", CodeAlpha3 = "VNM", CodeNumeric = 704, NameEnglish = "Viet Nam" },
            new CountryIso3166 { CodeAlpha2 = "VG", CodeAlpha3 = "VGB", CodeNumeric = 092, NameEnglish = "Virgin Islands (British)" },
            new CountryIso3166 { CodeAlpha2 = "VI", CodeAlpha3 = "VIR", CodeNumeric = 850, NameEnglish = "Virgin Islands (U.S.)" },
            new CountryIso3166 { CodeAlpha2 = "WF", CodeAlpha3 = "WLF", CodeNumeric = 876, NameEnglish = "Wallis and Futuna" },
            new CountryIso3166 { CodeAlpha2 = "EH", CodeAlpha3 = "ESH", CodeNumeric = 732, NameEnglish = "Western Sahara" },
            new CountryIso3166 { CodeAlpha2 = "YE", CodeAlpha3 = "YEM", CodeNumeric = 887, NameEnglish = "Yemen" },
            new CountryIso3166 { CodeAlpha2 = "ZM", CodeAlpha3 = "ZMB", CodeNumeric = 894, NameEnglish = "Zambia" },
            new CountryIso3166 { CodeAlpha2 = "ZW", CodeAlpha3 = "ZWE", CodeNumeric = 716, NameEnglish = "Zimbabwe" },
        };

        /// <summary>
        /// Get country by either alpha-2 or alpha-3 code
        /// </summary>
        /// <param name="alpha2OrAlpha3OrNumeric">Country code in either alpha-2, alpha-3 or numeric</param>
        /// <returns>The matching country or null</returns>
        public virtual CountryIso3166 Get(string alpha2OrAlpha3OrNumeric)
        {
            // the input could be numeric, but sent in as a string
            if (int.TryParse(alpha2OrAlpha3OrNumeric, out int numeric))
            {
                return Get(numeric);
            }

            var uppered = alpha2OrAlpha3OrNumeric.ToUpper();

            if (uppered.Length == 2)
            {
                return countries.FirstOrDefault(x => x.CodeAlpha2 == uppered);
            }

            return countries.FirstOrDefault(x => x.CodeAlpha3 == uppered);
        }

        /// <summary>
        /// Get country by numeric code
        /// </summary>
        /// <param name="numericCountryCode">The numeric country code</param>
        /// <returns>The matching country or null</returns>
        public virtual CountryIso3166 Get(int numericCountryCode)
        {
            return countries.FirstOrDefault(x => x.CodeNumeric == numericCountryCode);
        }

        /// <summary>
        /// Check whether two given country codes refer to the same country
        /// </summary>
        /// <param name="aAlpha2OrAlpha3OrNumeric">Country A</param>
        /// <param name="bAlpha2OrAlpha3OrNumeric">Country B</param>
        /// <returns>Whether or not the country codes refer to the same country. If one or both country codes are invalid, <c>false</c> is returned.</returns>
        public virtual bool Matches(string aAlpha2OrAlpha3OrNumeric, string bAlpha2OrAlpha3OrNumeric)
        {
            var a = Get(aAlpha2OrAlpha3OrNumeric);

            if (a == default(CountryIso3166))
            {
                return false;
            }

            return a.Is(bAlpha2OrAlpha3OrNumeric);
        }
    }
}