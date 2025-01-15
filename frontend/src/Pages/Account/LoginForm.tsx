import { ErrorMessage, Field, Form, Formik } from "formik";
import { Button, Container, FormField, Grid, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import "./LoginForm.css";
import { NavLink } from "react-router-dom";

export default observer(function LoginForm() {
  const { userStore } = useStore();

  return (
    <Container style={{ marginTop: "7em" }}>
      <Grid centered>
        <Grid.Column width={6}>
          <div className="login-segment">
            <h2 className="login-header">Welcome Back</h2>
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
                          className="login-input"
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
                          className="login-input"
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

                  <div className="login-buttons">
                    <Button positive type="submit" className="login-button">
                      Login
                    </Button>

                    <Button
                      type="button"
                      className="cancel-button"
                      as={NavLink}
                      to="/"
                    >
                      Cancel
                    </Button>
                  </div>

                  <div className="register-link">
                    <a href="/register">Don't have an account? Register</a>
                  </div>
                </Form>
              )}
            </Formik>
          </div>
        </Grid.Column>
      </Grid>
    </Container>
  );
});
