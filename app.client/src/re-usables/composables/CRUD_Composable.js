import { ref, onMounted } from 'vue';

const model = ref({});
const responseData = ref({});
const fm = ref({});
const valErrors = ref({});
const fetching = ref({
    isSuccess:null,
});
const layoutProps = ref({
    loader: {isLoading:false},
    errorModal: { isShow : false},
});
export function useFetch(modelName) {
    function submitForm(url, body) {
        layoutProps.value.loader.isLoading = true;
        fetching.value.isSuccess = false;
        layoutProps.value.errorModal.isShow = false;
        responseData.value = null;
        FnSetValidationErrors(model.value);
        //
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        };
        fetch(url, requestOptions)
            .then(response => {
                return response.json();
            })
            .then(data => {
                //handle validation Errors
                if (data && data.valErrors) {
                    valErrors.value = data.valErrors;
                    if (data.valErrors != null) {
                        let errors = Object.keys(data.valErrors);
                        errors.forEach(function (key1) {
                            valErrors.value[key1] = data.valErrors[key1];
                        });
                    }
                }
                //handle other errors
                else if (data && data.error) {
                    layoutProps.value.loader.isLoading = false;
                    layoutProps.value.errorModal.isShow = true;
                    layoutProps.value.errorModal.message = String(data.error);
                }
                //if no errors
                else {
                    responseData.value = data;
                    fetching.value.isSuccess = true;
                }
                layoutProps.value.loader.isLoading = false;
            }).catch((error) => {
                layoutProps.value.loader.isLoading = false;
                layoutProps.value.errorModal.isShow = true;
                layoutProps.value.errorModal.message = 'Oops! Something went wrong';
            });

    }
    function fetchModel() {
        layoutProps.value.loader.isLoading = true;
        fetching.value.isSuccess = false;
        model.value = null;
        layoutProps.value.errorModal.isShow = false;

        const requestOptions = {
            method: "GET",
        };
        fetch('/api/general/getmodel?model_name=' + modelName, requestOptions)
            .then(response => {
                return response.json();
            })
            .then(data => {
                if (data && data.Errors) {
                    layoutProps.value.errorModal.isShow = true;
                    layoutProps.value.errorModal.message = String(data.Errors);
                } else {
                    model.value = data;
                    FnSetValidationErrors(data);
                    fetching.value.isSuccess = true;
                }
                layoutProps.value.loader.isLoading = false;
            }).catch((error) => {
                layoutProps.value.loader.isLoading = false;
                layoutProps.value.errorModal.isShow = true;
                layoutProps.value.errorModal.message = String(error);
            });
    }
    //Get validation Errors
    function FnSetValidationErrors(myModel)
    {
        if (myModel != null) {
            let modelValues = Object.keys(myModel);
            modelValues.forEach(function (key1) {
                valErrors.value[key1] = null;
            });
        }
    }
    onMounted(() => {
        fetchModel();
    });
    //
    fm.value = {
        valErrors: valErrors,
        layoutProps: layoutProps,
        fetching: fetching,
        submitForm:submitForm,
        model: model,
        responseData: responseData,
    };
    return {fm};
}

