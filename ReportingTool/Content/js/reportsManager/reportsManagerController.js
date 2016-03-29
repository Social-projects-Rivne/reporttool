/// <reference path="../vendor/angular.js" />
'use strict';

reportsManagerModule.controller("reportsManagerController",
    ['$scope', '$stateParams', '$state', 'ReportsFactory', 'TempObjectFactory', '$filter', '$uibModal',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory, $filter, $uibModal) {

            var tempTemplate = TempObjectFactory.get();
            $scope.content = "team";
            $scope.templateValue = tempTemplate.templateName;

            $scope.teams = [{
                teamName: 'Loading...',
                teamID: ''
            }];

            $scope.templates = [{
                templateName: 'Loading...',
                templateId: -1
            }];

            $scope.selectedTeam = {};
            $scope.selectedTemplate = tempTemplate;

            ReportsFactory.getTeams().then(function (response) {
                $scope.teams = response.data;
            }, function (error) {
                console.error("getTeams error!", error);
            });

            ReportsFactory.getTemplates().then(function (response) {
                $scope.templates = response.data;
            }, function (error) {
                console.error("getTemplates error!", error);
            });

            $scope.jiraUsers = [{
                userName: 'Loading...',
                fullName: 'Loading...'
            }];

            ReportsFactory.getJiraUsers().then(getJiraUsersSuccess, getJiraUsersFail);

            function getJiraUsersSuccess(response) {
                $scope.jiraUsers = response.data;
            }

            function getJiraUsersFail(response) {
                console.error("getUsers error!");
            };

            $scope.members = [];

            $scope.cleanMembers=function() {
                $scope.members = [];
            }

            $scope.cleanTeam=function() {
                $scope.selectedTeam = {};
            }

            $scope.addMember = function (member) {
                for (var i in $scope.members) {
                    if ($scope.members[i].userName === member.userName) {
                        return;
                    }
                }
                $scope.members.push(member);
            }

            $scope.removeMember = function (userName) {
                for (var i in $scope.members) {
                    if ($scope.members[i].userName === userName) {
                        $scope.members.splice(i, 1);
                    }
                }
            }

            $scope.fromDate = new Date();
            $scope.toDate = new Date();

            $scope.clear = function () {
                $scope.from = null;
                $scope.to = null;
            };

            $scope.dateOptions = {
                dateDisabled: disabled,
                maxDate: new Date(2050, 5, 22),
                minDate: new Date(1990, 12, 31),
                startingDay: 1
            };

            function disabled(data) {
                var date = data.date,
                  mode = data.mode;
                return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
            }

            $scope.open1 = function () {
                $scope.popup1.opened = true;
            };

            $scope.open2 = function () {
                $scope.popup2.opened = true;
            };

            $scope.setDate = function (year, month, day) {
                $scope.dt = new Date(year, month, day);
            };

            $scope.popup1 = {
                opened: false
            };

            $scope.popup2 = {
                opened: false
            };

            function formatDate(date) {
                return $filter('date')(date, "yyyy-MM-dd");
            }

            function isEmpty(obj) {
                if (obj == null) return true;
                if (obj.length > 0) return false;
                if (obj.length === 0) return true;
                for (var key in obj) {
                    if (obj.hasOwnProperty(key))
                        return false;
                }
                return true;
            }

            function initializeTempReportConditionals(templateName, fields) {
                return {                  
                    templateName: templateName,
                    fields: fields,
                    members: $scope.members,
                    teamId: $scope.selectedTeam.teamID,
                    from: formatDate($scope.fromDate),
                    to: formatDate($scope.toDate)
                }
            }

            function saveReportConditionals(tempReportConditionals) {
                TempObjectFactory.set({});
                TempObjectFactory.set(tempReportConditionals);
                $state.go('reload');
               // $state.go('mainView.reportsManager.reportsConditions.reportDraft');
            }

            function reportDataValidation() {
                if (!(isEmpty($scope.members) ^ (!$scope.selectedTeam.teamID)))
                    return false;
                if (!$scope.selectedTemplate.templateName)
                    return false;
                var fromDate = Date.parse($scope.fromDate);
                var toDate = Date.parse($scope.toDate);
                if (fromDate > toDate) return false;
                return true;
            }

            function showValidationMessage(size) {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'modalContentValidation.html',
                    size: size
                });
            };

            $scope.saveIntoClientStorage = function () {
                if (reportDataValidation()) {
                    var dataPromise = ReportsFactory.getFields($scope.selectedTemplate.templateId);
                    dataPromise.then(function (result) {
                        var tempReportConditionals = initializeTempReportConditionals(result.data.templateName, result.data.fields);
                        saveReportConditionals(tempReportConditionals);
                    });
                } else {
                    showValidationMessage();
                }
            }

           
        }]);

reportsManagerModule.controller("reportDraftController",
    ['$scope', '$stateParams', '$state', 'ReportsFactory', 'TempObjectFactory',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory) {
           
            $scope.tempReport = TempObjectFactory.get();
            TempObjectFactory.set({});

//  ===================================================================================
            // an array of groups in the report
            $scope.reportedGroups = [];

            //  $scope.groups = {};
            //  $scope.activeGroup = {};

            //  a work template for group manipulation
            $scope.editGroup = {
                //groupID: "0",
                groupName: "",
                members: []
            };

            //  initial array of members for report
            $scope.reportedMembers = [{
                userName: 'Loading...',
                fullName: 'Loading...',
                role: 'No Role',
                activity: '2h',
                issues: []
            }];

            //  a number of async requests for progres indication
            //$scope.requestProcessing = false;
            $scope.requestProcessing = 0;

            $scope.addGroupPanel = false;

            //  get the list of members chosen for reporting
            function getAllMembers() {
                var tmp_members = [];
                
                // a team is present
                if ($scope.tempReport.teamId) {
                    //  for (var i in $scope.selectedTeam.members) {
                    //  for (var i = 0, len = $scope.selectedTeam.members.length; i < len; i++) {
                    for (var i = 0, len = $scope.selectedTeam.members.length; i < len; i++) {
                        var tmp_member = {};
                        tmp_member.userName = $scope.selectedTeam.members[i].userName;
                        tmp_member.fullName = $scope.selectedTeam.members[i].fullName;
                        tmp_member.role = '';
                        tmp_member.activity = 0;

                        tmp_members.push(tmp_member);
                    }
                }

                //  no team
                if (!$scope.tempReport.teamId) {
                    //  for (var i in $scope.selectedTeam.members) {
                    //  for (var i = 0, len = $scope.selectedTeam.members.length; i < len; i++) {
                    for (var i = 0, len = $scope.tempReport.members.length; i < len; i++) {
                        var tmp_member = {};
                        tmp_member.userName = $scope.tempReport.members[i].userName;
                        tmp_member.fullName = $scope.tempReport.members[i].fullName;
                        tmp_member.role = '';
                        tmp_member.activity = 0;

                        tmp_members.push(tmp_member);
                    }
                }
               
                return tmp_members;
            }
            $scope.reportedMembers = getAllMembers();

            //  get the list of members chosen for vacations
            $scope.vacationMembers = [];
            $scope.vacationMembers = getAllMembers();

            //  not done and not called
            function getActivityAndIssues() {
                for (var i = 0, len = $scope.reportedMembers.length; i < len; i++) {

                    $scope.getUserActivity($scope.reportedMembers[i].userName, $scope.tempReport.from, $scope.tempReport.to);
                    //$scope.getIssues($scope.reportedMembers[i].userName, $scope.tempReport.from, $scope.tempReport.to);
                }
            }

            //  a work template for member manipulation
            $scope.selectedMember = {
                userName: "",
                fullName: "",
                role: "",
                activity: "",
                issues: []
            };

            //  check if a specific section should be displayed
            $scope.showSection = function (sectionID) {
                for (var i in $scope.tempReport.fields) {
                    if ($scope.tempReport.fields[i].fieldID === sectionID) {
                        return true;
                    }
                }
                return false;
            };

            //  get a field from $scope.tempReport.fields
            $scope.getSingleField = function (sectionID) {
                for (var i in $scope.tempReport.fields) {
                    if ($scope.tempReport.fields[i].fieldID === sectionID) {
                        var tmp_field = {};

                        tmp_field.fieldDefaultValue = $scope.tempReport.fields[i].fieldDefaultValue;
                        tmp_field.fieldID = $scope.tempReport.fields[i].fieldID;
                        tmp_field.fieldName = $scope.tempReport.fields[i].fieldName;
                        tmp_field.fieldType = $scope.tempReport.fields[i].fieldType;
                        tmp_field.isSelected = $scope.tempReport.fields[i].isSelected;

                        return tmp_field;
                    }
                }
                return null;
            };

            //  show addGroupPanel ?
            $scope.addGroupPanel = false;

            //  add a member to a new group being set up
            $scope.addMember = function (member) {
                //  check to avoid member duplication
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === member.userName) {
                        return;
                    }
                }
                //  add the member to the group's list 
                var tmp_member = {};
                console.log("Adding member : " + member);
                tmp_member.userName = member.userName;
                tmp_member.fullName = member.fullName;
                tmp_member.role = member.role;
                tmp_member.activity = member.activity;
                // adding issues
                tmp_member.issues = member.issues;

                //$scope.editGroup.members.push(member);
                $scope.editGroup.members.push(tmp_member);

                //  remove the added member from initial report list
                for (var i in $scope.reportedMembers) {
                    if ($scope.reportedMembers[i].userName === member.userName) {
                        $scope.reportedMembers.splice(i, 1);
                        //  $scope.editGroup.members.splice(i, 1);
                    }
                }
            };

            //  remove a member from a new group being set up
            $scope.removeEditGroupMember = function (userName) {
                console.log('del member ' + userName);
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === userName) {
                        var tmp_member = {};

                        tmp_member.userName = $scope.editGroup.members[i].userName;
                        tmp_member.fullName = $scope.editGroup.members[i].fullName;
                        tmp_member.role = $scope.editGroup.members[i].role;
                        tmp_member.activity = $scope.editGroup.members[i].activity;
                        //  get the issues back
                        tmp_member.issues = $scope.editGroup.members[i].issues;

                        $scope.reportedMembers.push(tmp_member);

                        $scope.editGroup.members.splice(i, 1);
                    }
                }
            };

            //  a new group being set up : save
            $scope.save = function () {
                var tmp_group = {
                    groupName: "",
                    members: []
                };
                tmp_group.groupName = $scope.editGroup.groupName;

                //  remove the added member from initial report list
                for (var i in $scope.editGroup.members) {
                    for (var j in $scope.reportedMembers) {
                        if ($scope.editGroup.members[i].userName === $scope.reportedMembers[j].userName) {
                            $scope.reportedMembers.splice(j, 1);
                        }
                    }
                    tmp_group.members.push($scope.editGroup.members[i]);
                }

                //$scope.reportedGroups.push($scope.editGroup);
                $scope.reportedGroups.push(tmp_group);

                //  clear the editGroup obj
                $scope.editGroup.groupName = "";
                $scope.editGroup.members = [];

                $scope.addGroupPanel = false;
                //  $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
            }

            //  a new group being set up : cancel 
            $scope.cancel = function () {
                //  write the cancelled group members back to initial report list
                for (var i in $scope.editGroup.members) {
                    var tmp_member = {};

                    tmp_member.userName = $scope.editGroup.members[i].userName;
                    tmp_member.fullName = $scope.editGroup.members[i].fullName;
                    tmp_member.role = $scope.editGroup.members[i].role;
                    tmp_member.activity = $scope.editGroup.members[i].activity;
                    //  get the issues back
                    tmp_member.issues = $scope.editGroup.members[i].issues;

                    $scope.reportedMembers.push(tmp_member);
                    $scope.editGroup.members.splice(i, 1);
                }
                console.log("group save cancelled");
                $scope.addGroupPanel = false;
                //  $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
            }

            //  remove a group 
            $scope.removeGroup = function (groupId) {
                for (var i in $scope.reportedGroups[groupId].members) {
                    var tmpMember = $scope.reportedGroups[groupId].members[i];

                    $scope.reportedMembers.push(tmpMember);
                    //$scope.reportedGroups[groupId].members.splice(groupId, 1);
                }
                $scope.reportedGroups.splice(groupId, 1);
            };

            //  IN PROGRESS : remove a member from a group
            $scope.removeGroupMember = function (userName) {
                console.log('removeGroupMember : ' + userName);
                for (var j in $scope.reportedGroups) {
                    for (var i in $scope.reportedGroups[j].members) {

                        if ($scope.reportedGroups[j].members[i].userName === userName) {
                            var tmp_member = {};

                            tmp_member.userName = $scope.reportedGroups[j].members[i].userName;
                            tmp_member.fullName = $scope.reportedGroups[j].members[i].fullName;
                            tmp_member.role = $scope.reportedGroups[j].members[i].role;
                            tmp_member.activity = $scope.reportedGroups[j].members[i].activity;
                            //  get the issues back
                            tmp_member.issues = $scope.reportedGroups[j].members[i].issues;

                            $scope.reportedMembers.push(tmp_member);

                            $scope.reportedGroups[j].members.splice(i, 1);
                            return;
                        }
                    }
                }

            }

            $scope.groupMemberList = function (group) {
                var tmpMemberList = "";

                for (var i in group.members) {
                    if (tmpMemberList.length > 0) {
                        tmpMemberList += ", ";
                    }
                    tmpMemberList += group.members[i].fullName ;
                }
                tmpMemberList = "(" + tmpMemberList + ")";
                return tmpMemberList;
            };

            //  in progress !!! - is there any need ?
            $scope.groupIssueArray = function (group) {
                var tmpIssueArray = [];

                for (var i in  $scope.reportedGroups) {
                    for (var j in group.members.issues) {
                        var tmpIssue = group.members[i].issues[j];
                        tmpIssueArray.push(tmpIssue);
                    }
                }
                console.log("tmpIssueArray = " + tmpIssueArray);

                return tmpIssueArray;
            };

            $scope.getUserActivity = function (userName, dateFrom, dateTo) {

                //  debug
                console.log("userName = " + userName);
                console.log("dateFrom = " + dateFrom);
                console.log("dateTo = " + dateTo);

                //  hung up the browser
                //ReportsFactory.getUserActivity(userName, dateFrom, dateTo)
                //    .then(function (response) {
                //        //$scope.teams = response.data;
                //        userActivity = response.data;
                //    }, function (error) {
                //        console.error("getUserActivity error!", error);
                //    });

                //$scope.requestProcessing = true;
                $scope.requestProcessing += 1;

                ReportsFactory.getUserActivityRequest(userName, dateFrom, dateTo)
                .then(gUASuccess, gUAFail);

            };
                function gUASuccess(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    //  fill the activity field on scope
                    // $scope.time = (parseInt(response.data.Timespent) / 3600);
                    var seconds = response.data.Timespent;
                    var hours = Math.round( parseInt( response.data.Timespent) / 3600);
                    var userNameVar = response.data.userNameFromBE;

                    for (var i in $scope.reportedMembers) {
                        if ($scope.reportedMembers[i].userName === userNameVar) {
                            $scope.reportedMembers[i].activity = hours;
                        }
                    }

                }
                function gUAFail(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    alert("Error: " + response.code + ".  " + response.statusText);
                }
            //$scope.Test02 = $scope.getUserActivity("user1", "date1", "date2");

            $scope.getIssues = function (userName, dateFrom, dateTo) {
                //$scope.requestProcessing = true;
                $scope.requestProcessing += 1;

                ReportsFactory.getIssuesRequest(userName, dateFrom, dateTo)
                .then(gISuccess, gIFail);

            };
                function gISuccess(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    //  fill the issues field on scope
                    var issuesVar = response.data.Issues;
                    var userNameVar = response.data.userNameFromBE;

                    for (var i in $scope.reportedMembers) {
                        if ($scope.reportedMembers[i].userName === userNameVar) {
                            $scope.reportedMembers[i].issues = issuesVar;
                        }
                    }
                }
                function gIFail(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    alert("Error: " + response.code + ".  " + response.statusText);
                }
            //$scope.Test03 = $scope.getIssues("user1", "date1", "date2");

            //  get activity and issues for members for reportedMembers
                function getReportedMembersInfo() {
                    // if UserActivity is not checked in template --> return
                    var fid7 = 7;   //  fieldID of UserActivity div
                    if (!$scope.showSection(fid7)) {
                        return;
                    }

                    var from = $scope.tempReport.from;
                    var to = $scope.tempReport.to;

                    for (var i = 0, len = $scope.reportedMembers.length; i < len; i++) {

                        //  launch the activity Ajax request
                        var username = $scope.reportedMembers[i].userName;
                        //var from = $scope.tempReport.from;
                        //var to = $scope.tempReport.to;

                        $scope.getUserActivity(username, from, to);
                        $scope.getIssues(username, from, to);
                    }
                }
                getReportedMembersInfo();

            //  get activity for a PM
                $scope.pmMember = {};

                $scope.getPmActivity = function (userName) {
                    var dateFrom = $scope.tempReport.from;
                    var dateTo = $scope.tempReport.to;

                    //  debug
                    console.log("userName = " + userName);
                    console.log("dateFrom = " + dateFrom);
                    console.log("dateTo = " + dateTo);

                    $scope.requestProcessing += 1;

                    ReportsFactory.getUserActivityRequest(userName, dateFrom, dateTo)
                    .then(gPmASuccess, gPmAFail);

                };
                    function gPmASuccess(response) {
                        $scope.requestProcessing -= 1;

                        //  fill the activity field on scope
                        var seconds = response.data.Timespent;
                        var hours = Math.round(parseInt(response.data.Timespent) / 3600);
                        var userNameVar = response.data.userNameFromBE;

                        $scope.pmMember.userName = userNameVar;
                        $scope.pmMember.activity = hours;
                    }
                    function gPmAFail(response) {
                        $scope.requestProcessing -= 1;

                        alert("Error: " + response.code + ".  " + response.statusText);
                    }
            
            //  test User Activity for a fixed user
                //$scope.TestActivity = function () {
                //    console.log("TestActivity start");
                //        ReportsFactory.TestActivityRequest();
                //        console.log("TestActivity end");
                //        return 1;
                //}
                //$scope.TActivity = $scope.TestActivity();

            //  worked OK !
            //$scope.Test = ReportsFactory.getUserActivityRequest("user1", "date1", "date2");
               
            //  show VacationPanel ?
                $scope.addVacationPanel = false;

            // an array of vacations
            $scope.vacations = [];

            //  a vacation template for manipulation
            $scope.selectedVacation = {
                vacationID: "",
                userName: "",
                fullName: "",
                from: "",
                to: ""
            };

            //  a work template for member manipulation
            $scope.selectedMemberForVacation = {
                userName: "",
                fullName: ""
            };

            //  add a vacation record to vacation list
            $scope.addVacation = function (member, from, to) {
                var tmp_vacation = {};

                tmp_vacation.userName = member.userName;
                tmp_vacation.fullName = member.fullName;

                tmp_vacation.from = from.yyyymmdd();
                tmp_vacation.to = to.yyyymmdd();

                $scope.vacations.push(tmp_vacation);
            
            };

            $scope.removeVacation = function (id) {
                $scope.vacations.splice(id, 1);
            };

            Date.prototype.yyyymmdd = function () {
                var yyyy = this.getFullYear().toString();
                var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
                var dd = this.getDate().toString();
                return yyyy + "-" + (mm[1] ? mm : "0" + mm[0]) + "-" + (dd[1] ? dd : "0" + dd[0]); // padding
            };
            //d = new Date();
            //d.yyyymmdd();

            //  ========================================================================
            


            $scope.export = function () {
                var report = {
                    reporter: ""
                };

                window.open('/Reports/PreviewReport', '_blank', 'toolbar=1,resizable=0');
            };

        }]);

