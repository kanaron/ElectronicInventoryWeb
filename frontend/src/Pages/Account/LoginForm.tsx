import { Form, Formik } from "formik";
import React, { useState } from "react";
import { Button, FormInput } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function LoginForm() {
  const { userStore } = useStore();
  const [formData, setFormData] = useState({
    login: "",
    password: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Login Submitted:", formData);
    // Handle login logic here
  };

  return (
    <Formik
      initialValues={{ login: "", password: "" }}
      onSubmit={(values) => userStore.login(values)}
    >
      {({ handleSubmit, isSubmitting }) => (
        <Form onSubmit={handleSubmit} autoComplete="off">
          <FormInput label="Login" name="login" />
          <FormInput label="Password" name="password" />
          <Button loading={isSubmitting} positive type="submit" fluid>
            Login
          </Button>
        </Form>
      )}
    </Formik>
  );
});
