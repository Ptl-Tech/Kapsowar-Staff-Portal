import { useRouter } from 'vue-router';
import TomSelect from 'tom-select/src/tom-select.complete'
import * as css from 'tom-select/dist/css/tom-select.bootstrap4.css'
import { ref } from 'vue'
export function useFormComposable() {
    /**
     * REACTIVE VARIABLES
     */
    const xAutoSaveCounter = ref(0);
    const xInitialFormData = ref("");
    const xIsFormLoaded = ref(false);
    /**
     * FUNCTIONS
     */
    function loadTomSelects(ids) {
        var idList = ids.split(",");
        setTimeout(function () {
            for (var i = 0; i < idList.length; i++) {
                var ele = document.getElementById(idList[i]);
                if (ele != null) {
                    if (!ele.classList.contains("tomselected")) {
                        new TomSelect("#" + idList[i], {
                            allowEmptyOption: false,
                            create: false
                        });
                    }
                    else {
                        let control = ele.tomselect;
                        control.clearOptions();
                        control.sync();
                    }
                }
            }
        }, 1);
    }
    function setDropdownDims(response) {
        if (this.dims != undefined && response.dims.dims != undefined) {
            this.dims = response.dims.dims;
        }
        if (this.form.dimNos !== undefined && response.dims.dimNos !== undefined) {
            var nos = response.dims.dimNos.split(",");
            this.form.dimNos = nos;
            if (this.form.dimNos != null) {
                for (var i = 0; i < this.form.dimNos.length; i++) {
                    this.loadTomSelects('dim' + this.form.dimNos[i]);
                }
            }
        }
        if (this.dimNos !== undefined && response.dims.dimNos !== undefined) {
            var nos = response.dims.dimNos.split(",");
            this.dimNos = nos;
        }
    }
    function setFormDims(formData) {
        if (this.form.dimensionSet != undefined) {
            if (formData.Shortcut_Dimension_1_Code != undefined) {
                this.updateDimensionSet(1, formData.Shortcut_Dimension_1_Code);
            }
            else if (formData.Global_Dimension_1_Code != undefined) {
                this.updateDimensionSet(1, formData.Global_Dimension_1_Code);
            }
            //
            if (formData.Shortcut_Dimension_2_Code != undefined) {
                this.updateDimensionSet(2, formData.Shortcut_Dimension_2_Code);
            }
            else if (formData.Global_Dimension_2_Code != undefined) {
                this.updateDimensionSet(2, formData.Global_Dimension_2_Code);
            }
            //
            if (formData.Shortcut_Dimension_3_Code != undefined) {
                this.updateDimensionSet(3, formData.Shortcut_Dimension_3_Code);
            }
            //
            if (formData.Shortcut_Dimension_4_Code != undefined) {
                this.updateDimensionSet(4, formData.Shortcut_Dimension_4_Code);
            }
            //
            if (formData.Shortcut_Dimension_5_Code != undefined) {
                this.updateDimensionSet(5, formData.Shortcut_Dimension_5_Code);
            }
            //
            if (formData.Shortcut_Dimension_6_Code != undefined) {
                this.updateDimensionSet(6, formData.Shortcut_Dimension_6_Code);
            }
            //
            if (formData.Shortcut_Dimension_7_Code != undefined) {
                this.updateDimensionSet(7, formData.Shortcut_Dimension_7_Code);
            }
            //
            if (formData.Shortcut_Dimension_8_Code != undefined) {
                this.updateDimensionSet(8, formData.Shortcut_Dimension_8_Code);
            }
            //
            if (formData.Shortcut_Dimension_9_Code != undefined) {
                this.updateDimensionSet(9, formData.Shortcut_Dimension_9_Code);
            }
            //
            if (formData.Shortcut_Dimension_10_Code != undefined) {
                this.updateDimensionSet(10, formData.Shortcut_Dimension_10_Code);
            }
            //
            if (formData.Shortcut_Dimension_11_Code != undefined) {
                this.updateDimensionSet(11, formData.Shortcut_Dimension_11_Code);
            }
            //
            if (formData.Shortcut_Dimension_12_Code != undefined) {
                this.updateDimensionSet(12, formData.Shortcut_Dimension_12_Code);
            }
        }
    }
    function updateDimensionSet(dimNo, value) {
        if (this.form.dimensionSet != undefined) {
            this.form.dimensionSet["Dim_" + dimNo + "_Value"] = value;
        }
    }
    //used to save form data before navigating to a form line route
    //it only saves if the initial data is not the same as the current for data
    function xFnAutoSaveFormData(obj) {
        xAutoSaveCounter.value = xAutoSaveCounter.value + 1;
        if (xAutoSaveCounter.value == 1) {
            if (((xInitialFormData.value != JSON.stringify(this.form)) && obj.to.path.includes("/form/") && obj.status != undefined && obj.status == "Open") || (obj.saveAlways != undefined && obj.saveAlways == true && obj.to.path.includes("/form/"))) {
                this.$refs.actions.OnSubmit("save", { GuiAllowed: false, next: obj.next });
            } else {
                obj.next();
            }
        } else {
            obj.next();
        }
    }
    function xFnSetInitialFormData() {
        var self = this;
        setTimeout(function () {
            xInitialFormData.value = JSON.stringify(self.form);
        }, 2000);
    }
    function xOnAfterFormLoaded() {
        this.xIsFormLoaded = true;
    }
    const router = useRouter();
    //
    return {
        loadTomSelects,
        router, setDropdownDims,
        setFormDims, updateDimensionSet,
        xFnAutoSaveFormData, xFnSetInitialFormData, xIsFormLoaded, xOnAfterFormLoaded
    };
}
