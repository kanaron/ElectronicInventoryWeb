import { ErrorMessage, Field, Form, Formik } from "formik";
import { Button, Container, Grid } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import "./RegisterForm.css";

export default observer(function RegisterForm() {
  const { userStore } = useStore();

  return (
    <Container style={{ marginTop: "7em" }}>
      <Grid centered>
        <Grid.Column width={6}>
          <div className="register-segment">
            <h2 className="register-heared">Create an Account</h2>
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
                  <Field name="email">
                    {({ field }: any) => (
                      <input
                        {...field}
                        type="email"
                        placeholder="Email"
                        className="register-input"
                      />
                    )}
                  </Field>
                  <Field name="userName">
                    {({ field }: any) => (
                      <input
                        {...field}
                        placeholder="Username"
                        className="register-input"
                      />
                    )}
                  </Field>
                  <Field name="password">
                    {({ field }: any) => (
                      <input
                        {...field}
                        type="password"
                        placeholder="Password"
                        className="register-input"
                      />
                    )}
                  </Field>
                  <Field name="repeatPassword">
                    {({ field }: any) => (
                      <input
                        {...field}
                        type="password"
                        placeholder="Repeat Password"
                        className="register-input"
                      />
                    )}
                  </Field>

                  <ErrorMessage
                    name="error"
                    render={() =>
                      Array.isArray(errors.error) && (
                        <div className="register-error-messages">
                          {errors.error.map((err, idx) => (
                            <div key={idx} className="error-message">
                              {err}
                            </div>
                          ))}
                        </div>
                      )
                    }
                  />

                  <div className="register-buttons">
                    <Button posistive type="submit" className="register-button">
                      Register
                    </Button>

                    <Button
                      type="button"
                      className="cancel-button"
                      as={Link}
                      to="/"
                    >
                      Cancel
                    </Button>
                  </div>

                  <div className="login-link">
                    Already have an account?{" "}
                    <Link to="/login">Log in here</Link>
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
