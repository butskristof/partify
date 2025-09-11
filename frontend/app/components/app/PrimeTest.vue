<template>
  <div class="prime-test">
    <h2>prime test</h2>

    <div>
      <InputText placeholder="Enter a value here" />
    </div>

    <div>
      <InputText
        disabled
        placeholder="Disabled"
      />
    </div>

    <div class="buttons">
      <Button label="Submit" />
      <Button
        label="Profile"
        icon="pi pi-user"
      />
      <Button
        type="button"
        label="Search"
        icon="pi pi-search"
        :loading="true"
      />
      <ToggleSwitch />
    </div>

    <div class="progress-bar">
      <ProgressBar :value="50" />
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
.prime-test {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.buttons {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
