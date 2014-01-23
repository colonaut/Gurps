/// <reference path="angular-1.2.8/angular.js" />
/// <reference path="angular-1.2.8/angular-resource.js" />
/// <reference path="angular-1.2.8/angular-sanitize.js" />
/// <reference path="AlienDate.js" />

(function () {
    angular.module('alienDateModule', [])
        .directive('datepicker', ['$compile', function($compile) {
            return {
                restrict: 'E',
                replace: false,
                scope: {
                    cultureattr: '=culture',
                    calendarattr: '=calendar'
                },
                compile: function (tElement, tAttr) {
                    //compile the content within the element (we have isolated scope)
                    var compiledContents,
                        contents = tElement.contents().remove();

                    return function (scope, iElement, iAttr) {
                        if (!compiledContents)
                            compiledContents = $compile(contents);

                        compiledContents(scope, function (clone, scope) {
                            iElement.append(clone);
                        });
                    };
                },
                controller: [
                    '$scope', '$element',
                    function($scope, $element) {
                        $scope.month = AlienDate.month(new AlienDate('2000-4-5'));
                    }
                ],
                _link: function(scope, element, attrs) {
                    element.scope().month = AlienDate.month(new AlienDate('2000-4-5'));
                }
            };
        }]);
})();

(function() {

    angular.module('CollectionJsonServices', ['ngResource'])
        .factory('charactersService', ['appModel', '$resource',
            function (appModel, $resource) {

                var ngr = $resource('api/characters/:id', {}, {
                    query: { method: 'GET', params: {}, isArray: false },
                    get: { method: 'GET', params: {}, isArray: false },
                });

                return {
                    query: ngr.query,
                    get: ngr.get,
                    model: function() {
                        return appModel;
                    },
                };

            }
        ]);
}());

(function() {

        function run($timeout, startTime) {
            if (this.countdown <= 0) {
                if (this.autoreset())
                    this.$emit('timer-reset');
                this.isRunning = false;
                return;
            }
            this.isRunning = true;
            var $scope = this;
            $scope.timeout = $timeout(function() {
                if ($scope.countdown > 0) {
                    $scope.countdown = $scope.countdown - $scope.interval;
                    $scope.progress = 100 - (100 / $scope.duration) * $scope.countdown;
                }
                $scope.millis = new Date().getTime() - startTime;
                $scope.seconds = Math.floor(($scope.millis / 1000) % 60);
                $scope.minutes = Math.floor((($scope.millis / (1000 * 60)) % 60));
                $scope.hours = Math.floor((($scope.millis / (1000 * 60 * 60)) % 24));
                run.call($scope, $timeout, startTime);
            }, this.interval);
        }

        function reset($timeout) {
            this.isSet = true;
            this.isRunning = false;
            $timeout.cancel(this.timeout);
            this.countdown = this.duration;
            this.progress = 0;
            this.timeoutId = null;
            this.millis = 0;
            this.seconds = 0;
            this.minutes = 0;
            this.hours = 0;
            this.days = 0;
        }

        function init() {
            var unit, interval, duration, progress;
            if (!this.interval) {
                unit = this.durationattr.trim().replace(/\d/g, '');
                duration = parseInt(this.durationattr.trim(), 10);
                interval = this.intervalattr;
                switch (unit) {
                case 'ms':
                    this.interval = interval || 1;
                    this.duration = duration;
                    break;
                case 'm':
                case 'min':
                    this.interval = interval || 1000 * 60;
                    this.duration = duration * 1000 * 60;
                    break;
                case 'h':
                case 'hrs':
                    this.interval = interval || 1000 * 60 * 60;
                    this.duration = duration * 1000 * 60 * 60;
                    break;
                case 's':
                case 'sec':
                default:
                    this.interval = interval || 1000;
                    this.duration = duration * 1000;
                    break;
                }
            }
            this.countdown = 0;
            this.progress = 100;
        }

        angular.module('characterModule', ['CollectionJsonServices', 'alienDateModule'])
            //timer directive
            .directive('timer', [
                '$timeout', '$compile',
                function ($timeout, $compile) {
                    return {
                        restrict: 'E',
                        //replace: true,
                        //transclude: true,
                        //template: '<section data-ng-transclude></section>',
                        scope: {
                            intervalattr: '=interval',
                            //countdownattr: '=countdown',
                            durationattr: '=duration',
                            autoreset: '&'
                        },
                        compile: function(tElement, tAttr) {
                            //compile the content within the element (we have isolated scope)
                            var compiledContents,
                                contents = tElement.contents().remove();

                            return function(scope, iElement, iAttr) {
                                if (!compiledContents)
                                    compiledContents = $compile(contents);

                                $compile(contents)(scope, function (clone, scope) {
                                    iElement.append(clone);
                                });
                            };
                        },
                        controller: [
                            '$scope', '$element',
                            function($scope, $element) {
                                var startTime = null;
                                var pausedTime = null;
                                var resumedTime = null;

                                console.log($scope, 'controller');

                                $scope.isRunning = false;
                                $scope.isPaused = false;
                                $scope.isSet = false;

                                init.call($scope);
                                reset.call($scope, $timeout);

                                $scope.$on('timer-start', function(evt) {
                                    evt.preventDefault();
                                    evt.stopPropagation();
                                    reset.call($scope, $timeout);
                                    startTime = new Date().getTime();
                                    run.call($scope, $timeout, startTime);
                                });

                                $scope.$on('timer-pause', function(evt) {
                                    evt.preventDefault();
                                    evt.stopPropagation();
                                    $scope.isPaused = true;
                                    $timeout.cancel($scope.timeout);
                                    pausedTime = new Date().getTime();
                                    resumedTime = null;
                                });

                                $scope.$on('timer-resume', function(evt) {
                                    evt.preventDefault();
                                    evt.stopPropagation();
                                    $scope.isPaused = false;
                                    if (!pausedTime)
                                        return;
                                    startTime = new Date().getTime() - (pausedTime - startTime);
                                    pausedTime = null;
                                    resumedTime = new Date().getTime();
                                    run.call($scope, $timeout, startTime);
                                });

                                $scope.$on('timer-reset', function(evt) {
                                    evt.preventDefault();
                                    evt.stopPropagation();
                                    pausedTime = startTime = resumedTime = null;
                                    reset.call($scope, $timeout);
                                });

                                $scope.start = function() {
                                    console.log('start timer');
                                    $scope.$emit('timer-start');
                                };

                                $scope.pause = function() {
                                    console.log('pause timer');
                                    $scope.$emit('timer-pause');
                                };

                                $scope.resume = function() {
                                    console.log('resume timer');
                                    $scope.$emit('timer-resume');
                                };

                                $scope.reset = function() {
                                    console.log('reset timer');
                                    $scope.$emit('timer-reset');
                                };

                                $element.bind('$destroy', function() {
                                    $timeout.cancel($scope.timeoutId);
                                });

                                //implement autostartattribute

                            }
                        ]
                    };
                }
            ])

        //characterUpload directive
        .directive("characterUpload", ['$http', function($http) {
            if (!window.FileReader) {
                return {
                    restrict: 'E',
                    replace: true,
                    template: '<h5>Sorry, your browser does not support FileReader. Sucks. HTML5?</h5>'
                };
            }
            return {
                restrict: 'E',
                replace: true,
                transclude: true,
                template: '<div style="border:1px solid #00f;width:200px;height:100px;" ng-transclude>Drop file here</div>',
                scope: {
                    characters: '='
                },
                link: function(iScope, iElement) {
                    iElement.bind('dragenter', function() {
                        //console.log('standard dragenter');
                    }).bind('dragexit', function() {
                        //console.log('standard dragexit');
                    }).bind('dragover', function(event) {
                        event.preventDefault();
                        event.preventDefault();
                        //console.log('dragover: default prevented. propagation stopped;');
                    }).bind('drop', function(event) {
                        event.stopPropagation();
                        event.preventDefault();

                        var files = event.dataTransfer.files;

                        for (var i = 0, file; file = files[i]; i++) {
                            var reader = new FileReader();
                            reader.onload = (function(theFile) {
                                return function(evt) {
                                    //var foo = {
                                    //    name: theFile.name,
                                    //    type: theFile.type,
                                    //    size: theFile.size,
                                    //    lastModifiedDate: theFile.lastModifiedDate,
                                    //};
                                    var jsonCharacterTemplate = '{template: {entity:' + evt.target.result + '}}';
                                    var character = angular.fromJson(evt.target.result);

                                    //console.log(jsonCharacterTemplate);
                                    iScope.characters.push(character);

                                    $http.post(
                                        'api/character',
                                        jsonCharacterTemplate,
                                        {
                                            headers: {
                                                'Content-Type': 'application/json',
                                                'X-Requested-With': 'XMLHttpRequest'
                                            }
                                        }
                                    ).success(
                                        function(data) {
                                            console.log(data);
                                        }
                                    );
                                };
                            })(file);
                            reader.readAsText(file); //– returns the file contents as plain text
                            //reader.readAsBinaryString(file); //– returns the file contents as a string of encoded binary data (deprecated – use readAsArrayBuffer() instead)
                            //reader.readAsArrayBuffer(file); // – returns the file contents as an ArrayBuffer (good for binary data such as images)
                            //reader.readAsDataURL(file); // – returns the file contents as a data URL
                        }
                    });
                }
            };
        }])

        .controller(
            'PlayerCharactersController',
            [
                '$scope', 'charactersService',
                function playerCharacterController($scope, charactersService) {
                    var ix, il, l, entity, prop;

                    $scope.characters = [];

                    charactersService.query(function (r) {
                        l = r.collection.items;
                        for (ix = 0; il = l.length, ix < il; ix++) {
                            $scope.characters.push(l[ix].entity);
                        }
                        //console.log($scope.characters);
                    });

                    //$scope.playerCharacter = [];

                    //charactersService.get({ id: 1 }, function (r) {
                    //    entity = r.collection.items[0].entity;
                    //    for (prop in entity) {
                    //        $scope.playerCharacter[prop] = entity[prop];
                    //    }
                    //    //console.log($scope.playerCharacter);
                    //});


                    var foo = charactersService.get({ id: 1 }, function() {
                        $scope.playerCharacter = foo.collection.items[0].entity;
                        console.log($scope.playerCharacter);
                    });


                    console.log(charactersService.model());

                }
            ]
        );

    angular.module('cmsModule', [])
        .directive('editable', ['$compile', function ($compile) {
            var edittpl = '<div contenteditable="false">' +
                '<button ng-click="edit()">edit</button>' +
                '<button ng-click="save()">save</button>' +
                '</div>';
            return {
                restrict: 'C',
                replace: false,
                scope: {
                    contenteditable: '@'
                },
                controller: ['$scope', '$element', '$attrs',
                    function ($scope, $element, $attrs) {

                        function apply() {
                            $element.attr('contenteditable', $scope.contenteditable);
                        }

                        $scope.$on('edit', apply);
                        $scope.$on('save', apply);

                        $scope.edit = function () {
                            $scope.contenteditable = true;
                            $scope.$emit('edit');
                        };
                        $scope.save = function () {
                            $scope.contenteditable = false;
                            $scope.$emit('save');
                        };
                    }
                ],
                link: {
                    pre:
                        function (scope, element, attrs) {
                            element.prepend(edittpl);
                            $compile(element.contents())(scope);
                        }
                }
            };
        }]);

    angular.module('gameSessionModule', ['alienDateModule'])
        .directive('test', function() {
            return {
                restrict: 'A',
                scope:{}
            };
        })
        .directive('baseUri', [
            '$resource', '$compile',
            function ($resource, $compile) {
                var toolingHtml = '<div data-ng-form novalidate>' +
                        '<button ng-click="$coljex.template()">template</button>' +
                        '<section ng-if="$coljex.$active.template">' +
                            '<p>foo bar: the template</p>' +
                            '{{TEMPLATE}}' +
                            '<button ng-click="$coljex.cancelTemplate()">cancel</button>' +
                            '<button ng-click="$coljex.saveTemplate()">save</button>' +
                        '</section>' +
                    '</div>';
                return {
                    restrict: 'A',
                    //template: toolingHtml,
                    replace: false,
                    transclude: false,
                    //scope: {}, //this should not be isolated here,as we will have conflicts with other isolated directive scopes.... or should it be? YES! we can then have it on another item! check how form controller is done in angular...
                    link: {
                        post: function(scope, element, attrs) {
                            console.log(scope, 'directive post scope');
                            console.log(element.scope(), 'directive post element scope');

                            var di, ix, result;
                            function controls(data) {
                                result += '<ul>';
                                for (ix in data) {
                                    di = data[ix];
                                    result += '<li><label for="">' + di.prompt + '</label>';
                                    if (di.data)
                                        controls(di.data);
                                    else
                                        
                                        result += '<input type="text"></input>('+di.type+')';
                                    result += '</li>';
                                }
                                result += '</ul>';
                                
                            }
                            

                            $resource(attrs.baseUri).get({}, function (response) {
                                scope.$coljex.$base = response.collection;

                                controls(response.collection.template.data);
                                toolingHtml = toolingHtml.replace('{{TEMPLATE}}', result);

                                element.prepend(toolingHtml);
                                $compile(element.contents())(scope);
                            });
                        },
                        pre: function(scope, element, attrs) {
                            console.log(scope, 'directive pre scope');
                            console.log(element.scope(), 'directive pre element scope');

                            scope.$coljex = {
                                template: function() { this.$active.template = true; },
                                cancelTemplate: function() { this.$active.template = false; },
                                $active: {

                                }
                            };
                        }
                    }
                };
            }
        ])
        
        .factory('gameSessionsService', [
            '$resource',
            function($resource) {

                var ngr = $resource('foo/bar/api/gamesessions/:id', {}, {
                    query: { method: 'QUERY', params: {}, isArray: false },
                    get: { method: 'GET', params: {}, isArray: false },
                    post: { method: 'POST', params: {}, isArray: false },
                    put: { method: 'PUT', params: {}, isArray: false },
                    remove: { method: 'DELETE', params: {}, isArray: false },
                });

                return {
                    base: ngr.get,
                    query: ngr.query,
                    item: ngr.get,
                    create: ngr.post,
                    update: ngr.put,
                    remove: ngr.remove
                };

            }
        ])
        .controller('GameSessionsController', [
            '$scope', '$element', '$compile', 'gameSessionsService',
            function gameSessionsController($scope, $element, $compile, gameSessionsService) {
                console.log($scope, 'controller');
                var ix, items;
                
                $scope.items = [];

                $scope.create = function() {
                    console.log('$scope.create (should have template)');

                    gameSessionsService.create({}, function (createResponse, createHeaderFn) {
                        console.log('created');

                        console.log(createHeaderFn('location'), 'header location is the result of 201');

                        gameSessionsService.base(function(baseResponse) {
                            items = baseResponse.collection.items;
                            $scope.items.splice(0, $scope.items.length);
                            for (ix in items)
                                $scope.items.push(items[ix].entity);
                        });

                    });
                };


            }
        ]);


    

    angular.module('app', ['characterModule', 'gameSessionModule'])
        .value('appModel', {test:'available'});

})();
