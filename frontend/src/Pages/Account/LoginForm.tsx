import { Field, Form, Formik } from "formik";
import React, { useState } from "react";
import { Button, FormInput } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function LoginForm() {
  const { userStore } = useStore();

  return (
    <Formik
      initialValues={{ userName: "", password: "" }}
      onSubmit={(values) => {
        userStore.login(values);
      }}
    >
      {({ handleSubmit, isSubmitting }) => (
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

          <Button loading={isSubmitting} positive type="submit" fluid>
            Login
          </Button>
        </Form>
      )}
    </Formik>
  );
});
