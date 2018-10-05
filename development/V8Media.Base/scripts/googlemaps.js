(function ($, google, window) {

    var MapPoint = function (point) {
        $.extend(this, point);
    };

    MapPoint.prototype = {
        initialise: function (map) {
            this.map = map;
            return this;
        },
        addToMap: function () {
            return this.getLatLng()
                    .then(this.addMarker);
        },
        reMark: function () {
            return this.getLatLng()
                    .then(this.addMarker);
        },
        getLatLng: function () {
            var self = this;
            var defer = $.Deferred();
            var latLng = (this.Location || '0.0,0.0');
            latLng = latLng.replace(/ /g, '').split(',');
            if (latLng.length == 2 && !isNaN(parseFloat(latLng[0])) && !isNaN(parseFloat(latLng[1]))) {
                this.latLng = new google.maps.LatLng(latLng[0], latLng[1]);
                defer.resolveWith(self);
            }
            else {
                this.Location = this.Location.replace(',', ' ');
                this.map.geocoder.geocode({ 'address': this.Location }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        self.latLng = results[0].geometry.location;
                        defer.resolveWith(self);
                    }
                });
            }
            return defer.promise();
        },
        addMarker: function (point) {
            var map = this.map.googleMap;
            var self = this;
            if (!this.marker) {
                this.marker = new google.maps.Marker({
                    position: this.latLng,
                    title: this.Properties.Title || this.Properties.Name,
                    map: map
                });
                google.maps.event.addDomListener(this.marker, 'click', function () {
                    if (map.currentInfowindow) {
                        map.currentInfowindow.close();
                    }
                    var info = self.createInfoWindow(point);
                    info.open(map, self.marker);
                    map.currentInfowindow = info;
                });
            } else {
                this.marker.setPosition(this.latLng);
            }
            this.map.fitBounds();
        },
        removeMarker: function () {
            if (this.marker) {
                this.marker.setMap(null);
            }
        },
        createInfoWindow: function () {
            var url = this.Properties.Path || this.Properties.Url;
            var title = this.Properties.Title || this.Properties.Name;
            var content;
            if (!url) {
                content = '<h2>' + title + '</h2>';
            }
            else {
                content = '<h2><a href="' + url + '">' + title + '</a></h2>';
            }

            if (this.Properties.Description) {
                content += '<p>' + this.Properties.Description + '</p>';
            }
            var div = document.createElement('div');
            div.innerHTML = content;
            if (this.map.canGetDirections(this)) {
                var p = document.createElement('p');
                div.appendChild(p);
                var a = document.createElement('a');
                a.href = '#';
                a.innerHTML = this.map.settings.messages.getDirections;
                p.appendChild(a);
                google.maps.event.addDomListener(a, 'click', $.proxy(this.getDirections, this));
            }
            return new google.maps.InfoWindow({
                content: div
            });
        },
        getDirections: function () {
            this.map.getDirections(this);
            return false;
        }
    };

    var CurrentMapPoint = function (title) {
        MapPoint.call(this, { Properties: { Title: title } });
    };

    CurrentMapPoint.prototype = new MapPoint({});

    CurrentMapPoint.prototype.addToMap = function () {
        this.getCurrentLocation()
                .then(MapPoint.prototype.addToMap);
    };

    CurrentMapPoint.prototype.getCurrentLocation = function () {
        var self = this;
        var defer = $.Deferred();
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                self.Location = position.coords.latitude + ',' + position.coords.longitude;
                defer.resolveWith(self);
            });
        } else {
            defer.reject();
        }
        return defer.promise();
    };

    var Map = function (ids, settings) {
        this.ids = ids;
        this.settings = settings;
        this.points = [];
    };

    Map.prototype = {
        initialise: function () {
            var self = this;
            this.geocoder = new google.maps.Geocoder();
            this.directionsService = new google.maps.DirectionsService();
            this.directionsDisplay = new google.maps.DirectionsRenderer();

            var mapProps = {};
            this.googleMap = new google.maps.Map(document.getElementById(this.ids.map), mapProps);

            this.directionsDisplay.setMap(this.googleMap);

            if (this.settings.enableCurrentLocationResolution) {
                this.currentMapPoint = new CurrentMapPoint(this.settings.messages.youAreHere);
                this.currentMapPoint.initialise(this)
                    .addToMap();
            }
            $.each(this.points, function (index, point) {
                point.initialise(self)
                        .addToMap();
            });

            return this;
        },
        addPoint: function (point) {
            this.points[this.points.length] = new MapPoint(point);
            return this;
        },
        setCurrentPoint: function (point) {
            if (this.currentMapPoint) {
                this.currentMapPoint.Location = point.Location.replace(',', '');
                this.currentMapPoint.reMark();
            }
            else {
                this.currentMapPoint = new MapPoint(point);
                this.currentMapPoint.initialise(this).addToMap();
            }
        },
        fitBounds: function () {
            var bounds = new google.maps.LatLngBounds();
            var addToBounds = function (point) {
                if (point && point.latLng) {
                    bounds.extend(point.latLng);
                }
            };
            if (this.currentMapPoint) {
                addToBounds(this.currentMapPoint);
            }

            $.each(this.points, function (index, point) {
                addToBounds(point);
            });

            if (this.points.length === 1) {
                this.googleMap.setCenter(this.points[0].latLng);
                this.googleMap.setZoom(12);
            } else {
                this.googleMap.fitBounds(bounds);
            }
        },
        getDirections: function (point) {
            var directionsDisplay = this.directionsDisplay;
            var ids = this.ids;
            var request = {
                origin: this.currentMapPoint.latLng,
                destination: point.latLng,
                travelMode: this.settings.TravelMode || google.maps.TravelMode.DRIVING
            };
            this.directionsService.route(request, function (result, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(result);
                    directionsDisplay.setPanel(document.getElementById(ids.directions));
                }
            });
        },
        canGetDirections: function (point) {
            if (!this.settings.enableDirections) {
                return false;
            }
            if (this.currentMapPoint && this.currentMapPoint.latLng) {
                return point != this.currentMapPoint;
            }
            return false;
        }
    };

    window.v8media = window.v8media || {};

    window.v8media.map = function (points, settings) {
        var ids = {};
        $.each(['map', 'directions', 'findMe', 'findMeText'], function () {
            ids[this] = settings.idPrefix + this;
        });

        var map = new Map(ids, settings);
        if (points) {
            $.each(points, function () {
                map.addPoint(this);
            });
        }
        google.maps.event.addDomListener(window, 'load', function () { map.initialise(); });

        if (settings.enableLocationSearch) {
            $('#' + ids.findMe).bind('click', function () {
                var locationToFind = $('#' + ids.findMeText).val();
                if (locationToFind) {
                    map.setCurrentPoint({ Properties: { Title: settings.messages.youAreHere }, Location: locationToFind });
                }
                return false;
            });
        }
        return map;
    };

})(jQuery, google, window);

