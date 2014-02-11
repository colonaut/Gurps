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


    angular.module('cmsModule', [])
        .directive('editable', [
            '$compile', function($compile) {
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
                    controller: [
                        '$scope', '$element', '$attrs',
                        function($scope, $element, $attrs) {

                            function apply() {
                                $element.attr('contenteditable', $scope.contenteditable);
                            }

                            $scope.$on('edit', apply);
                            $scope.$on('save', apply);

                            $scope.edit = function() {
                                $scope.contenteditable = true;
                                $scope.$emit('edit');
                            };
                            $scope.save = function() {
                                $scope.contenteditable = false;
                                $scope.$emit('save');
                            };
                        }
                    ],
                    link: {
                        pre:
                            function(scope, element, attrs) {
                                element.prepend(edittpl);
                                $compile(element.contents())(scope);
                            }
                    }
                };
            }
        ]);

    function structure(entity, data) {


        for (var ix in data) {
            console.log(data[ix]);
        }

    }
    angular.module('collectionJsonModule', [])
        //modified include reading contents and use it as inline template... #-400
        .directive('inlineInclude', [
            '$compile', '$templateCache',
            function($compile, $templateCache) {
                return {
                    restrict: 'A',
                    priority: -800,
                    require: 'inlineInclude',
                    compile: function(element, attr) {
                        if (!$templateCache.get(attr.modInclude))
                            $templateCache.put(attr.modInclude, element.html());

                        //var contents =
                        element.contents().remove(); // we need to do this!

                        return function(scope, $element, $attr, ctrl) {
                            $element.html(ctrl.template);
                            $compile($element.contents())(scope);
                        };
                    }
                };
            }
        ])
        //modified include readingreading cintents and use it as inline template... #400
        .directive('inlineInclude', [
            '$http', '$templateCache', '$anchorScroll', '$animate',
            function($http, $templateCache, $anchorScroll, $animate) {
                return {
                    restrict: 'A',
                    priority: 400,
                    terminal: true,
                    transclude: 'element',
                    controller: angular.noop,
                    compile: function(element, attr) {
                        return function(scope, $element, $attr, ctrl, $transclude) {
                            var currentScope,
                                currentElement,
                                newScope = scope.$new();

                            ctrl.template = $templateCache.get($attr.modInclude);

                            var clone = $transclude(newScope, function(clone) {
                                if (currentScope) {
                                    currentScope.$destroy();
                                    currentScope = null;
                                }
                                if (currentElement) {
                                    $animate.leave(currentElement);
                                    currentElement = null;
                                }

                                $animate.enter(clone, null, $element);
                            });

                            currentScope = newScope;
                            currentElement = clone;
                        };
                    }
                };
            }
        ])
        //upload sestructured json
        .directive('upload', [
            //'',
            function() {
                return {
                    restrict: 'A',
                    scope: true,
                    link: function (scope, element, attrs) {

                        console.log(attrs.upload, 'upload');

                        element.bind('dragenter', function () {
                            }).bind('dragexit', function () {
                            }).bind('dragover', function(event) {
                                event.preventDefault();
                                event.preventDefault();
                            }).bind('drop', function (event) {
                                event.stopPropagation();
                                event.preventDefault();
                                var files = event.dataTransfer.files,
                                    ix, file;
                                for (ix = 0; file = files[ix]; ix++) {
                                    var reader = new FileReader();
                                    reader.onload = (function(theFile) {
                                        return function(evt) {
                                            //var foo = {
                                            //    name: theFile.name,
                                            //    type: theFile.type,
                                            //    size: theFile.size,
                                            //    lastModifiedDate: theFile.lastModifiedDate,
                                            //};
                                            //var jsonCharacterTemplate = '{template: {entity:' + evt.target.result + '}}';

                                            var obj = angular.fromJson(evt.target.result);

                                            console.log(structure(obj, scope.collection.template.data));



                                            
                                            console.log(obj);

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
            }
        ])
        //collection as NOT isolated directive dealing with the next controller
        .directive('collectionJson', [
            '$compile',
            function($compile) {
                var dataItemTypes = ['values', 'value', 'options', 'select', 'object', 'objects'],
                    ix,
                    keyx;

/**/
                function getDataItemType(dataItem) {
                    for (ix in dataItemTypes)
                        if (dataItem.hasOwnProperty(dataItemTypes[ix]))
                            return dataItemTypes[ix];
                    return null;
                };

                function destructure(data) {
                    var result = {};
                    for (ix in data) {
                        keyx = data[ix].name;

                        //console.log(keyx);

                        //switch (getDataItemType(data[ix])) {
                        //    case 'value':
                        //        result[keyx] = null;
                        //        break;
                        //    case 'object':
                        //        result[keyx] = destructure(data[ix].data);
                        //        break;
                        //    case 'values':
                        //    case 'objects':
                        //        result[keyx] = [];
                        //        break;

                        //}
                    }
                    return result;
                }


                

                /* the directive */
                return {
                    restrict: 'A',
                    replace: false,
                    scope: true,
                    controller: [
                        '$scope', '$element', '$attrs', '$http',
                        function($scope, $element, $attrs, $http) {

                            console.log($scope, 'collection-json scope');

                            $scope.random = Math.random();

                            $scope.getDataItemType = function(property) {
                                return getDataItemType(property);
                            };

                            $scope.collection = {
                                $add: function(objects, data) {
                                    objects.push({ data: angular.copy(data) });
                                },
                                $remove: function(objects, objectItem) {
                                    objects.splice(objects.indexOf(objectItem), 1);
                                },
                                $structure: function(entity) {
                                    return structure(entity);
                                }
                                
                            };

                            //add object to objects collection of data structure (will be depr?)
                            $scope.addObject = function(objects, data) {
                                objects.push({ data: angular.copy(data) });
                            };
                            //will be depr ?
                            $scope.removeObject = function(objects, objectItem) {
                                objects.splice(objects.indexOf(objectItem), 1);
                            };

                            //post a filled template (TODO: required stuff...)
                            $scope.create = function() {
                                console.log('post the template now...');
                                console.log($scope.template, "template to post");
                            };

                            //load data...
                            console.log('load collection data');
                            $http.get($attrs.collectionJson)
                                .success(function(data, status, headers, config) {
                                    angular.extend($scope.collection, data.collection);                                    
                                })
                                .error(function(data, status, headers, config) {
                                });

                        }
                    ]
                };
            }
        ]);


    angular.module('characterModule', ['collectionJsonModule', 'alienDateModule'])
        //timer directive
        .directive('timer', [
            '$timeout', '$compile',
            function($timeout, $compile) {
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

                            $compile(contents)(scope, function(clone, scope) {
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

                            //console.log($scope, 'controller');

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
        .controller(
            'PlayerCharactersController',
            [
                '$scope',
                function playerCharacterController($scope) {
                    var ix, il, l, entity, prop;

                    console.log($scope, 'playerCharacterController scope');

                    console.log($scope.collection);

                    //charactersService.query(function (r) {
                    //    l = r.collection.items;
                    //    for (ix = 0; il = l.length, ix < il; ix++) {
                    //        $scope.characters.push(l[ix].entity);
                    //    }
                    //    //console.log($scope.characters);
                    //});

                    //$scope.playerCharacter = [];

                    //charactersService.get({ id: 1 }, function (r) {
                    //    entity = r.collection.items[0].entity;
                    //    for (prop in entity) {
                    //        $scope.playerCharacter[prop] = entity[prop];
                    //    }
                    //    //console.log($scope.playerCharacter);
                    //});


                    //var foo = charactersService.get({ id: 1 }, function() {
                    //    $scope.playerCharacter = foo.collection.items[0].entity;
                    //    console.log($scope.playerCharacter);
                    //});


                }
            ]
        );

    
    //TODO: do we want enity? or should we use the vonversion type.... and have the dentity directly on items and therfore no data...?


    angular.module('gameSessionModule', ['collectionJsonModule', 'alienDateModule'])
        //resource service (depreceated)
        .factory('gameSessionsService', [
            '$resource',
            function($resource) {

                var ngr = $resource('api/gamesessions/:id', {}, {
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
            '$scope', '$element', '$compile',
            function gameSessionsController($scope, $element, $compile) {
                //console.log($scope, 'GameSessionsController');
                var ix, sessions;

                $scope.sessions = [];

                $scope.create = function() {
                    //console.log('$scope.create (should have template)');

                    //gameSessionsService.create({}, function(createResponse, createHeaderFn) {
                    //    //console.log('created');
                    //    //console.log(createHeaderFn('location'), 'header location is the result of 201');

                    //    gameSessionsService.base(function(baseResponse) {
                    //        sessions = baseResponse.collection.items;
                    //        $scope.sessions.splice(0, $scope.sessions.length);
                    //        for (ix in sessions)
                    //            $scope.sessions.push(sessions[ix].entity);
                    //    });

                    //});
                };


            }
        ]);


    

    angular.module('app', ['characterModule', 'gameSessionModule'])
        .value('appModel', {test:'available'});

})();
