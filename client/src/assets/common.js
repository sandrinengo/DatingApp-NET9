/*
 *
 *   Dating App
 *   author: Sandrine Ngo
 *   created on : 12/15/2024 
 *   last update: 
*/
function CapitalizeFirstWord(text) {
    if (!text) {
        return ""; // Handle empty string case
    }
    return text.charAt(0).toUpperCase() + text.slice(1);
}