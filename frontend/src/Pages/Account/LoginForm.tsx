import { ErrorMessage, Field, Form, Formik } from "formik";
import { Button, FormInput, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function LoginForm() {
  const { userStore } = useStore();

  return (
    <Formik
      initialValues={{ userName: "", password: "", error: null }}
      onSubmit={(values, { setErrors }) => {
        userStore
          .login(values)
          .catch((error) => setErrors({ error: "Invalid login or password" }));
      }}
    >
      {({ handleSubmit, errors }) => (
        <Form onSubmit={handleSubmit} autoComplete="off">
          <Field name="userName">
            {({ field }: any) => (
              <FormInput
                {...field}
                label="Login"
                placeholder="Enter your login"
              />
            )}
          </Field>

          <Field name="password">
            {({ field }: any) => (
              <FormInput
                {...field}
                label="Password"
                placeholder="Enter your password"
                type="password"
              />
            )}
          </Field>

          <ErrorMessage
            name="error"
            render={() => (
              <Label
                style={{ marginBottom: 10 }}
                basic
                color="red"
                content={errors.error}
              />
            )}
          />

          <Button positive type="submit" fluid>
            Login
          </Button>
        </Form>
      )}
    </Formik>
  );
});
