﻿function isIterable(obj) {
  // checks for null and undefined
  if (obj == null) {
    return false;
  }
  return typeof obj[Symbol.iterator] === "function";
}

function findFromObject(obj, search) {
  if (!isIterable(obj)) {
    return undefined;
  }
  for (const item of obj) {
    if (search(item)) {
      return item;
    }
  }
}

function deleteFromObject(obj, search) {
  if (!isIterable(obj)) {
    return undefined;
  }
  let index = -1;
  for (const item of obj) {
    index++;
    if (search(item)) {
      return obj.filter((x, i) => i !== index);
    }
  }
  return obj;
}

function updateFromObject(obj, newItem, search) {
  if (!isIterable(obj)) {
    return undefined;
  }
  let index = -1;
  for (const item of obj) {
    index++;
    if (search(item)) {
      obj[index] = newItem;
    }
  }
}

function addItemInObject(obj, item) {
  if (!isIterable(obj)) {
    return undefined;
  }
  obj.push(item);
}

// ["a", "b", "c"]

// [
//  {name: "test", quantity: 10},
//]
