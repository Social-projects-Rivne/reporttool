var reportsManagerModule = angular.module("reportsManagerModule", ['ui.router', 'ui.bootstrap',
   //    'templatesManagerModule',
    'ngAnimate']);

reportsManagerModule.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    var reportsManager = {
        name: 'mainView.reportsManager',
        url: '/reportsManager',
        cache: false,
        views: {
            '': {
                templateUrl: 'Content/templates/reportsManager/reportsManagerView.html',
                controller: 'reportsManagerController'
            }
        }
    };

    var reportConditions = {
        name: 'mainView.reportsManager.reportsConditions',
        url: '/reportsConditions',
        templateUrl: 'Content/templates/reportsManager/reportsConditions.html',
        controller: 'reportsManagerController'
    };

    var reportDraft = {
        name: 'mainView.reportsManager.reportsConditions.reportDraft',
        url: '/reportDraft',
        templateUrl: 'Content/templates/reportsManager/reportDraft.html',
        controller: 'reportDraftController'
    };


    $stateProvider.state(reportsManager);
    $stateProvider.state(reportConditions);
    $stateProvider.state(reportDraft);
}]);
