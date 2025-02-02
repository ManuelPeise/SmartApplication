export type KeysOfType<T, V> = {
  [K in keyof T]: T[K] extends V ? K : never;
}[keyof T];

export type KeysOfTypeBool<TModel> = KeysOfType<TModel, boolean>;

export type KeysOfTypeNumber<TModel> = KeysOfType<TModel, number>;

export type KeysOfTypeString<TModel> = KeysOfType<TModel, string>;

export type KeysOfTypeObject<TModel> = KeysOfType<TModel, object>;

export type KeysOfTypeFunction<TModel> = KeysOfType<TModel, "function">;
