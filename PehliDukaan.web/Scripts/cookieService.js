

function addCookieItem(item, cookieName, path = "/") {
    $.cookie(cookieName, item, { path: path });
}

function getCookieItem(cookieName) {
    return $.cookie(cookieName);
}

function addCookieJsonItems(items, cookieName, path = "/") {
    const jsonString = JSON.stringify(items);
    addCookieItem(jsonString, cookieName, path);
}

function getCookieJsonItems(cookieName) {
    const jsonString = getCookieItem(cookieName);
    return jsonString ? JSON.parse(jsonString) : undefined;
}