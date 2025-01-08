import { ErrorMessage, Field, Form, Formik } from "formik";
import { Button, Container, FormField, Grid, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function LoginForm() {
  const { userStore } = useStore();

  return (
    <Container style={{ marginTop: "7em" }}>
      <Grid centered>
        <Grid.Column width={6}>
          <Formik
            initialValues={{ userName: "", password: "", error: null }}
            onSubmit={(values, { setErrors }) => {
              userStore
                .login(values)
                .catch((error) =>
                  setErrors({ error: "Invalid login or password" })
                );
            }}
          >
            {({ handleSubmit, errors }) => (
              <Form onSubmit={handleSubmit} autoComplete="off">
                <FormField>
                  <Field name="userName">
                    {({ field }: any) => (
                      <input
                        {...field}
                        placeholder="Enter your login"
                        style={{ width: "100%", padding: "10px" }}
                      />
                    )}
                  </Field>
                </FormField>

                <FormField style={{ marginTop: "1em" }}>
                  <Field name="password">
                    {({ field }: any) => (
                      <input
                        {...field}
                        type="password"
                        placeholder="Enter your password"
                        style={{ width: "100%", padding: "10px" }}
                      />
                    )}
                  </Field>
                </FormField>

                <ErrorMessage
                  name="error"
                  render={() => (
                    <Label
                      style={{ marginBottom: "10px", marginTop: "1em" }}
                      basic
                      color="red"
                      content={errors.error}
                    />
                  )}
                />

                <Button
                  positive
                  type="submit"
                  fluid
                  style={{ marginTop: "1.5em", padding: "12px" }}
                >
                  Login
                </Button>
              </Form>
            )}
          </Formik>
        </Grid.Column>
      </Grid>
    </Container>
  );
});
