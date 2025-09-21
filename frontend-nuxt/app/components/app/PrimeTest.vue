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

    <div class="progress-bars">
      <ProgressBar :value="50" />
      <ProgressBar
        class="styled-progress-bar"
        :value="50"
        :show-value="false"
      />
    </div>

    <div class="colors">
      <div class="color-block primary" />
      <div class="color-block secondary" />
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
.prime-test {
  display: flex;
  flex-direction: column;
  gap: var(--default-spacing);
}

.buttons {
  display: flex;
  flex-wrap: wrap;
  gap: var(--default-spacing);
}

.progress-bars {
  display: flex;
  flex-direction: column;
  gap: var(--default-spacing);

  .styled-progress-bar {
    --p-progressbar-height: 0.5rem;
    --p-progressbar-value-background: linear-gradient(
      to right,
      var(--color-primary),
      var(--color-secondary)
    );
  }
}

.colors {
  display: flex;
  flex-direction: row;
  gap: var(--default-spacing);

  .color-block {
    width: 20px;
    height: 20px;

    &.primary {
      background-color: var(--color-primary);
    }

    &.secondary {
      background-color: var(--color-secondary);
    }
  }
}
</style>
