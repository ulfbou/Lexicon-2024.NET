var DOMUtils = {};

DOMUtils.create = function(tagName, className, id) {
  var element = document.createElement(tagName);

  if (className) {
    element.classList.add(className);
  }

  if (id) {
    element.id = id;
  }

  return element;
};

DOMUtils.append = function(parent, element) {
  parent.appendChild(element);
};

DOMUtils.remove = function(element) {
  element.remove();
};

DOMUtils.get = function(selector) {
  return document.querySelector(selector);
};

DOMUtils.getAll = function(selector) {
  return document.querySelectorAll(selector);
};

DOMUtils.on = function(element, eventName, callback) {
    const target = typeof element === 'string' ? document.querySelector(element) : element;
  
    if (target) {
      target.addEventListener(eventName, callback);
    } else {
      console.error(`Element not found: ${element}`);
    }
};

DOMUtils.off = function(element, eventName, callback) {
    const target = typeof element === 'string' ? document.querySelector(element) : element;
    
    if (target) {
        target.removeEventListener(eventName, callback);
    } 
    else {
        console.error(`Element not found: ${element}`);
    }
};

DOMUtils.emit = function(element, eventName, data) {
  var event = new CustomEvent(eventName, { detail: data });
  element.dispatchEvent(event);
};
