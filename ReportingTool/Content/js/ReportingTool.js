"use strict"

var ReportingTool = angular.module('ReportingTool', ['ui.router', 'mainViewModule']); //add to dependencies: loginModule, configureModule

//Routing for loginView and configureView

//ReportingTool.config(['$stateProvider', function ($stateProvider) {
//    var login = {
//        name: 'login',
//        url: '/login',
//        templateUrl: 'loginView.html',
//        controller: 'loginController'
//    },
//        config = {
//            name: 'config',
//            url: '/config',
//            templateUrl: 'content.red.html',
//            controller: 'configController'
//        };

//    $stateProvider.state(login);
//    $stateProvider.state(config);
//}]);


ReportingTool.run(['$rootScope', '$state', '$stateParams',
    function ($rootScope,   $state,   $stateParams) {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

    if (true) {                                     //  if (isConfigureFileValid)
        if (true) {                              //  if (isLoggedIn)
            // ReportingTool.requires.push('manageTeamsModule');
            $state.transitionTo('mainView');
        }
        else $state.transitionTo('loginView');
    }
    else $state.transitionTo('configView');
}]);