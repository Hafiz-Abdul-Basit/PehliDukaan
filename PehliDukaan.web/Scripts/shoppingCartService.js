const cookieName = "CartProducts";

function getShoppingCartItems() {
  return getCookieJsonItems(cookieName) ?? [];
}

function getProductFromShoppingCart(productId) {
  const cartItems = getShoppingCartItems();

  return findFromObject(cartItems, (item) => item.id === parseInt(productId));
}

function addProductInShoppingCart(productId, quantity = 1) {
  const cartItems = getShoppingCartItems();

  addItemInObject(cartItems, {
    id: parseInt(productId),
    quantity: parseInt(quantity),
  });
  addCookieJsonItems(cartItems, cookieName);
}

function updateProductInShoppingCart({ id, quantity }) {
    const cartItems = getShoppingCartItems();

  updateFromObject(cartItems, { id, quantity }, x => x.id === parseInt(id));
  addCookieJsonItems(cartItems, cookieName);
}

function setProductQuantityInShoppingCart(productId, quantity) {
  const product = getProductFromShoppingCart(productId);
  product.quantity = parseInt(quantity);
  updateProductInShoppingCart(product);
}

function increateProductQuantityInShoppingCart(productId, quantity = 1) {
  const product = getProductFromShoppingCart(productId);
  product.quantity = product.quantity + parseInt(quantity);
  updateProductInShoppingCart(product);
}

function decreaseProductQuantityInShoppingCart(productId, quantity = 1) {
  const product = getProductFromShoppingCart(productId);
  product.quantity = product.quantity - parseInt(quantity);

  if (product.quantity <= 0) {
    removeProductFromShoppingCart(productId);
  } else {
    updateProductInShoppingCart(product);
  }
}

function addOrUpdateProductInShoppingCart(productId, quantity = 1) {
  const product = getProductFromShoppingCart(productId);
  // If product doesn't exist add it
  if (!product) {
    addProductInShoppingCart(productId, quantity);
    return;
  }

  increateProductQuantityInShoppingCart(productId, quantity);
}

function removeProductFromShoppingCart(productId) {
  const cartItems = getShoppingCartItems();
  const existingProduct = getProductFromShoppingCart(productId);
  if (!existingProduct) {
    console.error("Unable to find product");
    return;
  }

  const filteredItems = deleteFromObject(
    cartItems,
    (x) => x.id === parseInt(productId)
  );
  addCookieJsonItems(filteredItems, ProductCartCookieName);
}

function getProductCountOfShoppingCart() {
  return getShoppingCartItems().length;
}
