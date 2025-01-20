export type FormSetup<TModel> = {
  id: string;
  formLabel: string;
  sections: FormField<TModel>[];
};

export type FormField<TModel> = {
  id: keyof TModel;
  label: string;
  value: TModel[keyof TModel];
  fields: FormField<TModel[keyof TModel]>[];
};

// function GenericForm<T>({ fields, onSubmit }: GenericFormProps<T>) {
//   const [values, setValues] = useState<T>(
//     fields.reduce((acc, field) => {
//       acc[field.name] = field.value;
//       return acc;
//     }, {} as T)
//   );
//   const [errors, setErrors] = useState<{ [key: string]: string }>({});

//   const handleChange = (name: string, value: any) => {
//     setValues((prev) => ({
//       ...prev,
//       [name]: value,
//     }));
//     setErrors((prev) => ({
//       ...prev,
//       [name]: '',
//     }));
//   };

//   const handleSubmit = (e: React.FormEvent) => {
//     e.preventDefault();

//     let formValid = true;
//     let newErrors: { [key: string]: string } = {};

//     fields.forEach((field) => {
//       const error = field.validate(values[field.name]);
//       if (error) {
//         newErrors[field.name] = error;
//         formValid = false;
//       }

//       if (field.nestedFields) {
//         field.nestedFields.forEach((nestedField) => {
//           const nestedError = nestedField.validate(values[field.name][nestedField.name]);
//           if (nestedError) {
//             newErrors[`${field.name}.${nestedField.name}`] = nestedError;
//             formValid = false;
//           }
//         });
//       }
//     });

//     if (formValid) {
//       onSubmit(values);
//     } else {
//       setErrors(newErrors);
//     }
//   };

// onChange={(e) =>
//                     handleChange(`${field.name}.${nestedField.name}`, e.target.value)
//                   }
