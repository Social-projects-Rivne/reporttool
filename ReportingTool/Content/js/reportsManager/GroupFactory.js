'use strict';

//  ---  from TeamFactory ------------------------------------------------------------------   //  debug

reportsManagerModule.
factory('GroupFactory', ['$http', function ($http) {

    var groupFactory = {
        getAllGroups: getAllGroups,
        getAllMembers: getAllMembers,
        updateGroup: updateGroup,
        deleteGroup: deleteGroup,
        createGroup: createGroup
    };

    function getAllGroups() {
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

    function deleteGroup(deleteGroupName) {
        //return $http.delete("Teams/DeleteTeam", { params: { id: id } });
        console.log('delete group ' + deleteGroupName);

        for (var i in $scope.reportedGroups) {
            if ($scope.reportedGroups[i].groupName === deleteGroupName) {
                $scope.reportedGroups.splice(i, 1);
            }
        }
    }

    return groupFactory;
}]);

//  ---------------------------------------------------------------------------------------------------  //  debug
