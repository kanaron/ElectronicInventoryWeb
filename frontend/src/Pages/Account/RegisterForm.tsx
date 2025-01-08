import { ErrorMessage, Field, Form, Formik } from "formik";
import { Button, Container, FormField, Grid, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function RegisterForm() {
  const { userStore } = useStore();

  return (
    <Container style={{ marginTop: "7em" }}>
      <Grid centered>
        <Grid.Column width={6}>
          <Formik
            initialValues={{
              email: "",
              userName: "",
              password: "",
              repeatPassword: "",
              error: [],
            }}
            onSubmit={(values, { setErrors }) => {
              if (values.password !== values.repeatPassword) {
                setErrors({ error: ["Passwords do not match"] });
                return;
              }
              userStore.register(values).catch((error) => {
                if (Array.isArray(error)) {
                  setErrors({ error: error.map((err) => err.description) });
                } else {
                  setErrors({ error: ["An unexpected error occurred"] });
                }
              });
            }}
          >
            {({ handleSubmit, errors }) => (
              <Form onSubmit={handleSubmit} autoComplete="off">
                {/* Email Field */}
                <FormField>
                  <Field name="email">
                    {({ field }: any) => (
                      <input
                        {...field}
                        type="email"
                        placeholder="Enter your email"
                        style={{ width: "100%", padding: "10px" }}
                      />
                    )}
                  </Field>
                </FormField>

                {/* Username Field */}
                <FormField style={{ marginTop: "1em" }}>
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

                {/* Password Field */}
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

                {/* Repeat Password Field */}
                <FormField style={{ marginTop: "1em" }}>
                  <Field name="repeatPassword">
                    {({ field }: any) => (
                      <input
                        {...field}
                        type="password"
                        placeholder="Repeat your password"
                        style={{ width: "100%", padding: "10px" }}
                      />
                    )}
                  </Field>
                </FormField>

                {/* Error Message */}
                <ErrorMessage
                  name="error"
                  render={() =>
                    Array.isArray(errors.error) && (
                      <div style={{ marginTop: "1em" }}>
                        {errors.error.map((err, idx) => (
                          <Label
                            key={idx}
                            style={{ marginBottom: "5px" }}
                            basic
                            color="red"
                            content={err}
                          />
                        ))}
                      </div>
                    )
                  }
                />

                {/* Submit Button */}
                <Button
                  positive
                  type="submit"
                  fluid
                  style={{ marginTop: "1.5em", padding: "12px" }}
                >
                  Register
                </Button>
              </Form>
            )}
          </Formik>
        </Grid.Column>
      </Grid>
    </Container>
  );
});
