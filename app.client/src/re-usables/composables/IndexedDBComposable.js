export function useIndexedDB() {
    async function IDB_GetSetDims(dimNos, dims) {
        var dimsObj = {};
        for (var i = 0; i < dimNos.length; i++) {
            var dimNo = dimNos[i];
            var dimNoDims = await IDB_GetDims(dimNo);
            if (dimNoDims[0] != undefined && dimNoDims != null && dimNoDims != "") {
                dimsObj["dims" + dimNo] = dimNoDims;
            } else {
                var isSet = await IDB_SetDims(dimNo, dims["dims" + dimNo]);
                var dimNoDims2 = await IDB_GetDims(dimNo);
                dimsObj["dims" + dimNo] = dimNoDims2;
            }
        }
        return dimsObj;
    }
    async function IDB_SetDims(dimNo, dims) {
        console.log(dims);
        return new Promise((resolve, reject) => {
            if (dims = "") { return; }
            // This works on all devices/browsers, and uses IndexedDBShim as a final fallback 
            var indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB || window.shimIndexedDB;
            indexedDB.deleteDatabase("dims" + dimNo);
            // Open (or create) the database
            var open = indexedDB.open("dims" + dimNo, 1);
            // Create the schema
            open.onupgradeneeded = function () {
                var db = open.result;
                const store = db.createObjectStore("MyObjectStore", { keyPath: "dimNo" });
                store.createIndex("DimsIndex", ["dims.Code", "dims.Name"]);
            };
            open.onsuccess = function () {
                // Start a new transaction
                var db = open.result;
                //if (db.objectStoreNames.contains("MyObjectStore")) {
                var tx = db.transaction("MyObjectStore", "readwrite");
                var store = tx.objectStore("MyObjectStore");
                //var index = store.index("DimsIndex");
                if (dims != null) {
                    store.put({ dimNo: dimNo, dims: dims });
                }
                tx.oncomplete = function () {
                    resolve(true);
                    db.close();
                };
                //}
            }
        });
    }
    async function IDB_GetDims(dimNo) {
        return new Promise((resolve, reject) => {
        // Access IndexedDB
        const indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB || window.shimIndexedDB;
        // Open (or create) the database
            const open = indexedDB.open("dims" + dimNo, 1);
        let dims = {};
        open.onsuccess = function () {
            // Start a new transaction
            const db = open.result;
            if (db.objectStoreNames.contains("MyObjectStore")) {
                const tx = db.transaction("MyObjectStore", "readonly");
                const store = tx.objectStore("MyObjectStore");
                const getDims = store.get(dimNo);
                getDims.onsuccess = function () {
                    dims = getDims.result?.dims;
                    db.close();
                    resolve(dims);
                };
                getDims.onerror = function () {
                    db.close();
                    reject(new Error("Failed to get dims from IndexedDB")); // Reject the promise on error
                };
                tx.oncomplete = function () {
                    db.close();
                };
            } else {
                resolve(dims);
            }
        };
        open.onerror = function () {
            reject(new Error("Failed to open IndexedDB")); // Reject the promise if unable to open the DB
        };
        });
    }
    function IDB_DeleteDims() {
        var indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB || window.shimIndexedDB;
        for (var i = 1; i <= 12;i++) {
            indexedDB.deleteDatabase("dims" +i);
        }
    }
    return { IDB_GetSetDims, IDB_DeleteDims };
}


