import React from "react";

const UPDATE_ACTION = "UPDATE";
const RESET_ACTION = "RESET";

type UpdateFunction<T> = {
  type: typeof UPDATE_ACTION;
  payload: T;
};

type ResetFunction<T> = {
  type: typeof RESET_ACTION;
  payload: T;
};

type FormActionTypes<T> = UpdateFunction<T> | ResetFunction<T>;

const formReducer = <T>(initialState: T, action: FormActionTypes<T>): T => {
  switch (action.type) {
    case UPDATE_ACTION:
      return { ...initialState, ...action.payload };
    case RESET_ACTION:
      return { ...initialState, ...action.payload };
    default:
      return initialState;
  }
};

export const useFormState = <T>(
  initialState: T,
  validationCallback?: (model: T) => boolean
) => {
  const originalState = React.useRef(initialState);

  const [state, dispatch] = React.useReducer<
    React.Reducer<T, FormActionTypes<T>>
  >(formReducer, initialState);

  const isModified = React.useMemo((): boolean => {
    const keys = Object.keys(state) as Array<keyof T>;

    return keys.some((k) => {
      return state[k] !== originalState.current[k];
    });
  }, [state, originalState]);

  const subScribe = React.useCallback((): {
    formState: T;
    isDirty?: boolean;
    isValid?: boolean;
  } => {
    return {
      isDirty: isModified,
      formState: state,
      isValid: validationCallback
        ? validationCallback(state) ||
          JSON.stringify(originalState.current) === JSON.stringify(state)
        : undefined,
    };
  }, [state, isModified, validationCallback]);

  const updatePartial = React.useCallback(
    (partialStateUpdate: Partial<T>) => {
      dispatch({
        type: "UPDATE",
        payload: { ...state, ...partialStateUpdate },
      });
    },
    [state, dispatch]
  );

  const update = React.useCallback(
    (stateUpdate: T) => {
      dispatch({
        type: "UPDATE",
        payload: stateUpdate,
      });
    },
    [dispatch]
  );

  const reset = React.useCallback(() => {
    dispatch({ type: "RESET", payload: originalState.current });
  }, [originalState, dispatch]);

  return {
    subScribe,
    handleUpdatePartial: updatePartial,
    handleUpdate: update,
    handleReset: reset,
  };
};
