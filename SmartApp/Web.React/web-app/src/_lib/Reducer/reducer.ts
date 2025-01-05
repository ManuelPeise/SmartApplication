export const isNotEqualToPrev = (a: unknown, b: unknown): boolean => {
  if (a === b) {
    return false;
  }

  if (Array.isArray(a)) {
    if (Array.isArray(b) && a.length === 0 && b.length === 0) {
      return false;
    }
  } else if (a != null && a.constructor.name === "Object") {
    if (
      b != null &&
      b.constructor.name === "Object" &&
      Object.keys(a).length === 0 &&
      Object.keys(b).length === 0
    ) {
      return false;
    }
  }

  return true;
};

export const reducerFunction = <TModel>(
  prevState: TModel,
  stateUpdate: Partial<TModel> | ((prevState: TModel) => Partial<TModel>)
): TModel => {
  const newPartState: Partial<TModel> =
    typeof stateUpdate === "function" ? stateUpdate(prevState) : stateUpdate;

  if (newPartState == null) {
    return null;
  }

  let stateChanged = false;

  if (prevState == null) {
    stateChanged = newPartState != null;
  } else {
    const objectKeys = Object.keys(newPartState) as Array<keyof TModel>;

    stateChanged = objectKeys.some((key) => {
      const isModified = isNotEqualToPrev;

      const equal = isModified(prevState[key], newPartState[key]);

      console.log("Equal:", equal);

      return equal;
    });
  }

  if (stateChanged) {
    return {
      ...prevState,
      ...newPartState,
    };
  }

  return prevState;
};
