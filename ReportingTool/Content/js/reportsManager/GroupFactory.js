'use strict';

//  ---  from TeamFactory ------------------------------------------------------------------   //  debug

reportsManagerModule.
factory('GroupFactory', ['$http', function ($http) {

    var groupFactory = {
        GetAllGroups: GetAllGroups,
        getAllMembers: getAllMembers,
        updateGroup: updateGroup,
        deleteGroup: deleteGroup,
        createGroup: createGroup
    };

    function GetAllGroups() {
        //var teams = $http.get("Teams/GetAllTeams");
        var groups = $scope.groups;
        return groups;
    }

    function getAllMembers() {
        //var teams = $http.get("Teams/GetAllTeams");
        var members = $scope.reportedMembers;
        return members;
    }

    function createGroup(newGroup) {
        //return $http({
        //    url: 'Teams/AddNewTeam',
        //    method: 'POST',
        //    data: newTeam,
        //    headers: { 'content-type': 'application/json' }
        //});
        var tmpGroup = {};
        tmpGroup = newGroup;
        return tmpGroup;
    }

    function updateGroup(data) {
        //return $http.put("Groups/EditTeam", data);
        //  TODO ? - there is no need to update. create-delete would be enough
    }

    function deleteGroup(id) {
        //return $http.delete("Teams/DeleteTeam", { params: { id: id } });
    }

    return groupFactory;
}]);

//  ---------------------------------------------------------------------------------------------------  //  debug
