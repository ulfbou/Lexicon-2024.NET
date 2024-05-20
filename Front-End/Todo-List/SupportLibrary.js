var Library = {};

/**
 * Simple event emitter and listener functions to handle custom events.
 */
Library.EventEmitter = function(
) {
  var listeners = {};

  this.on = function(eventName, callback) {
    if (!listeners[eventName]) {
      listeners[eventName] = [];
    }
    listeners[eventName].push(callback);
  };

  this.once = function(eventName, callback) {
    var self = this;
    this.on(eventName, function(
) {
      self.off(eventName, callback);
      callback.apply(this, arguments);
    });
  };

  this.off = function(eventName, callback) {
    if (!listeners[eventName]) {
      return;
    }
    var index = listeners[eventName].indexOf(callback);
    if (index !== -1) {
      listeners[eventName].splice(index, 1);
    }
  };

  this.emit = function(eventName, data) {
    var callbacks = listeners[eventName];
    if (callbacks) {
      callbacks.forEach(function(callback) {
        callback(data);
      });
    }
  };
};

/**
 * Object utilities
 */
Library.ObjectUtils = {};

Library.ObjectUtils.extend = function(target, source) {
  for (var key in source) {
    if (source.hasOwnProperty(key)) {
      target[key] = source[key];
    }
  }
  return target;
};

Library.ObjectUtils.keys = function(obj) {
  var keys = [];
  for (var key in obj) {
    if (obj.hasOwnProperty(key)) {
      keys.push(key);
    }
  }
  return keys;
};

/**
 * Array utilities
 */
Library.ArrayUtils = {};

Library.ArrayUtils.forEach = function(array, callback) {
  for (var i = 0; i < array.length; i++) {
    callback(array[i]);
  }
};
