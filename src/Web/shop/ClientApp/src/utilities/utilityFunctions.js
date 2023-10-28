export const replaceItemById = (array, id, newItem) => {
  const index = array.findIndex(item => item.id === id);

  if (index === -1) return array;

  array[index] = newItem;
  
  return array;
}