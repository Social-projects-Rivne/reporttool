'use strict'

mainViewModule.factory('TempObjectFactory', function () {

    var tempObject = {};

    var tempObjectProp = {
        get: get,
        set: set
    };

    function get() {
        return tempObject;
    }

    function set(selectedObject) {
        tempObject = selectedObject;
    }

    return tempObjectProp;
});