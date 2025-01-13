import { ref } from 'vue'
export function useGeneralComposable() {
    function cplFnNavDateObjToString(navDateObj){
        return navDateObj.Day + "-" + navDateObj.Month + "-" + navDateObj.Year;
    }
    function cplFnNavDateObjToISODate(navDateObj) {
        var date = new Date(navDateObj.Year, navDateObj.Month, navDateObj.Day).toISOString().split("T")[0];
        return date;
    }
    return {
        cplFnNavDateObjToString, cplFnNavDateObjToISODate
    };
}
